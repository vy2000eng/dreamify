using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dreamify.API.Handlers;
using dreamify.Application.Abstracts;
using dreamify.Application.Services;
using dreamify.Domain.Entities;
using dreamify.Infrastructure;
using dreamify.Infrastructure.Options;
using dreamify.Infrastructure.Processors;
using dreamify.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using dreamify.API.Controllers;
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.Configure<JwtOptions> (builder.Configuration.GetSection(JwtOptions.JwtOptionsKey));
builder.Services.Configure<OpenAiOptions> (builder.Configuration.GetSection(OpenAiOptions.OpenApiOptionsKey));


builder.Services.AddIdentity<User, IdentityRole<Guid>>(opt =>
{
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequiredLength = 8;
    opt.User.RequireUniqueEmail = true;
    opt.User.AllowedUserNameCharacters = 
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";

}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseMySQL(builder.Configuration.GetConnectionString("DbConnectionString"));
});

builder.Services.AddScoped<IAuthTokenProcessor,AuthTokenProcessor>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountService,AccountService>();
builder.Services.AddScoped<IOpenAiRequestProcessor, OpenAiRequestProcessor>();
builder.Services.AddScoped<IOpenAiService, OpenAiService>();
builder.Services.AddScoped<IGoogleTokenProcessor, GoogleTokenProcessor>();


builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    }
)
.AddJwtBearer(options =>
{
    var jwtOption = builder.Configuration.GetSection(JwtOptions.JwtOptionsKey)
        .Get<JwtOptions>() ?? throw new ArgumentException(nameof(JwtOptions));

    options.TokenValidationParameters = new TokenValidationParameters
    {
        
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOption.Issuer,
            ValidAudience = jwtOption.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Secret))
        
    };
    //Console.WriteLine($"Validation - Secret first 10 chars: {jwtOption.Secret.Substring(0, 10)}");
    Console.WriteLine($"Validation - Issuer: {jwtOption.Issuer}");
    Console.WriteLine($"Validation - Audience: {jwtOption.Audience}");
    
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            Console.WriteLine($"Exception type: {context.Exception.GetType().Name}");
            if (context.Exception.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {context.Exception.InnerException.Message}");
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token validated successfully!");
            Console.WriteLine($"Principal identity name: {context.Principal.Identity.Name}");
            Console.WriteLine($"Principal identity authenticated: {context.Principal.Identity.IsAuthenticated}");
            Console.WriteLine($"Claims count: {context.Principal.Claims.Count()}");
            
            // Debug: Print all claims
            foreach (var claim in context.Principal.Claims)
            {
                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }
            // Force set the HttpContext user
            context.HttpContext.User = context.Principal;
            
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            Console.WriteLine("=== JWT MESSAGE RECEIVED ===");
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                context.Token = authHeader.Substring("Bearer ".Length).Trim();
                Console.WriteLine("Token extracted successfully");
            }
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.WithTitle("dreamify API");
    });
}

app.UseCors("AllowOrigins");
app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.UseRouting(); 
app.Use(async (context, next) =>
{
    Console.WriteLine($"=== MIDDLEWARE DEBUG: {context.Request.Method} {context.Request.Path} ===");
    Console.WriteLine($"Authorization header exists: {context.Request.Headers.ContainsKey("Authorization")}");
    await next.Invoke();
    Console.WriteLine($"=== MIDDLEWARE DEBUG END: Response Status {context.Response.StatusCode} ===");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
