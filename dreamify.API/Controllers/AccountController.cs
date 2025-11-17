using System.Security.Claims;
using dreamify.Application.Abstracts;
using dreamify.Domain.Entities;
using dreamify.Domain.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;

namespace dreamify.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountController:ControllerBase
{
    private readonly IAccountService _accountService;


    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<IResult> Register(RegisterRequest request)
    {
        var registerResponse = await _accountService.RegisterUserAsync(request);

        return Results.Ok(registerResponse);
    }
    [HttpPost("login")]
    public async Task<IResult> LoginRequest(LoginRequest request)
    {
        var loginResponse = await _accountService.LoginUserAsync(request);

        return Results.Ok(loginResponse);
    }
    [HttpPost("refresh")]
    public async Task<IResult> RefreshRequest(RefreshRequest request)
    {
        var refreshResponse = await _accountService.RefreshTokenAsync(request);

        return Results.Ok(refreshResponse);
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("userInfo")]
    public async Task<IResult> GetUserInfo()
    {
        
        var refreshResponse = await _accountService.GetUserInfoAsync(User);

        return Results.Ok(refreshResponse);
    }
    
    
    
    
    
    
}
