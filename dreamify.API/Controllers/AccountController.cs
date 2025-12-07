using System.Security.Claims;
using dreamify.Application.Abstracts;
using dreamify.Domain.Entities;
using dreamify.Domain.Requests;
using dreamify.Domain.Response;
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
        //TODO: add try catches
        try
        {
            var registerResponse = await _accountService.RegisterUserAsync(request);

            return Results.Ok(registerResponse);

        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);

            
            
        }
      
    }
    [HttpPost("login")]
    public async Task<IResult> LoginRequest(LoginRequest request)
    {
        //TODO: add try catches

        try
        {
            var loginResponse = await _accountService.LoginUserAsync(request);

            return Results.Ok(loginResponse);

        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
            
        }

        
    }
    [HttpPost("refresh")]
    public async Task<IResult> RefreshRequest(RefreshRequest request)
    {
        //TODO: add try catches
        try
        {
            var refreshResponse = await _accountService.RefreshTokenAsync(request);

            return Results.Ok(refreshResponse);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
            
        }

       
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("userInfo")]
    public async Task<IResult> GetUserInfo()
    {
        try
        {
            var userInfo = await _accountService.GetUserInfoAsync(User);
            return Results.Ok(userInfo);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("UpdateUserInfo")]
    public async Task<IResult> UpdateUserInfo(InfoRequest request)
    {
        try
        {
            await _accountService.UpdateUserInfoAsync(request, User);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
    
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("DeleteUser")]
    public async Task<IResult> DeleteUserAccount()
    {
        try
        {
            var response = await _accountService.DeleteUserInfoAsync(User);
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            var response = new DeleteUserResponse()
            {
                ResultMessage = ex.Message
            };
            return Results.BadRequest(response);
        }
    }


    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("SignInWithGoogle")]
    public async Task<IResult> LoginWithGoogle(GoogleLoginRequest request)
    {
        try
        {
            var response = await _accountService.LoginWithGoogle(request);
            return Results.Ok(response);


        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
        
    }
    
    
    
    
    
    
}
