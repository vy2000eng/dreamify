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
        //TODO: add try catches
        var registerResponse = await _accountService.RegisterUserAsync(request);

        return Results.Ok(registerResponse);
    }
    [HttpPost("login")]
    public async Task<IResult> LoginRequest(LoginRequest request)
    {
        //TODO: add try catches

        var loginResponse = await _accountService.LoginUserAsync(request);

        return Results.Ok(loginResponse);
    }
    [HttpPost("refresh")]
    public async Task<IResult> RefreshRequest(RefreshRequest request)
    {
        //TODO: add try catches

        var refreshResponse = await _accountService.RefreshTokenAsync(request);

        return Results.Ok(refreshResponse);
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("userInfo")]
    public async Task<IResult> GetUserInfo()
    {
        //TODO: add try catches

        
        var userInfo = await _accountService.GetUserInfoAsync(User);

        return Results.Ok(userInfo);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("UpdateUserInfo")]
    public async Task<IResult> UpdateUserInfo(InfoRequest request)
    {
        //TODO: add try catches
        try
        {
            await _accountService.UpdateUserInfoAsync(request, User);
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
        
        
        
    }
    
    
    
    
    
    
}
