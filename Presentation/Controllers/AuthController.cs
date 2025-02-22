using Business.Dtos.Auth;
using Business.Services.Abstract;
using Business.Services.Concrete;
using Business.Utilities.EmailHandler.Abstract;
using Business.Utilities.EmailHandler.Models;
using Business.Wrappers;
using Common.Entities;
using Common.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public AuthController(IAuthService authService,
          UserManager<User> userManager,
          IEmailService emailService)
    {
        _authService = authService;
        _userManager = userManager;
        _emailService = emailService;
    }
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return BadRequest("Invalid email");
        if (user.EmailConfirmed)
            return Ok("Email already confirmed.");

        Console.WriteLine("Received Token: " + token);
        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest($"Invalid token or confirmation failed: {errors}");
        }
       

        return Ok("Email successfully confirmed!");
    }
        #region Documentation
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="model"></param>
        #endregion
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    [HttpPost("register")]
    public async Task<Response> RegisterAsync([FromForm] AuthRegisterDto model)
        => await _authService.RegisterAsync(model);

    #region Documentation
    /// <summary>
    /// Login
    /// </summary>
    /// <param name="model"></param>
    [ProducesResponseType(typeof(Response<AuthLoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    #endregion
    [HttpPost("Login")]
    public async Task<Response<AuthLoginResponseDto>> LoginAsync([FromForm] AuthLoginDto model)
    => await _authService.LoginAsync(model);




    #region Documentation
    /// <summary>
    /// LogOut
    /// </summary>
    /// <param name="model"></param>
    #endregion
    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Logout()
    {

        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(new { Message = "Invalid or missing token." });
        }


        await _authService.LogoutAsync(token);


        return Ok(new { Message = "User successfully logged out." });
    }

}
