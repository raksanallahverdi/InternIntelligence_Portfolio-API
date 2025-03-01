﻿using AutoMapper;
using Business.Dtos.Auth;

using Business.Services.Abstract;
using Business.Utilities.EmailHandler.Abstract;
using Business.Utilities.EmailHandler.Models;
using Business.Validators.Auth;
using Business.Wrappers;
using Common.Entities;
using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Common.Exceptions;

namespace Business.Services.Concrete;

public class AuthService : IAuthService
{
    private readonly ITokenBlacklistService _tokenBlacklistService;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailService _emailService;

    public AuthService(ITokenBlacklistService tokenBlacklistService,
        UserManager<User> userManager,
                        IMapper mapper,
                        IConfiguration configuration,
                     
                        IHttpContextAccessor httpContextAccessor,
                        IEmailService emailService)
    {
        _tokenBlacklistService = tokenBlacklistService;
        _userManager = userManager;
        _mapper = mapper;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _emailService = emailService;
    }
    public async Task LogoutAsync(string token)
    {
        Console.WriteLine("Logging out and blacklisting token: " + token);

        await _tokenBlacklistService.AddToBlacklistAsync(token);

      
        Console.WriteLine("User logged out successfully.");
    }

    public async Task<Response> RegisterAsync(AuthRegisterDto model)
    {
        var result = await new AuthRegisterDtoValidator().ValidateAsync(model);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
            throw new ValidationException("User already exists");
      

        user = _mapper.Map<User>(model);
        var registerResult = await _userManager.CreateAsync(user, model.Password);
        if (!registerResult.Succeeded)
            throw new ValidationException(registerResult.Errors.Select(x => x.Description));



        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        Console.WriteLine("Generated Token: " + confirmationToken);




        // Get the base URL dynamically
        var request = _httpContextAccessor.HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
        Console.WriteLine(baseUrl);
        string encodedToken = Uri.EscapeDataString(confirmationToken);
        var confirmationUrl = $"{baseUrl}/api/auth/confirm-email?token={encodedToken}&email={user.Email}";

        var message = new Message(
            new List<string> { model.Email },
            "Email Confirmation",
            $"Please confirm your email by clicking the link below: {confirmationUrl}"
        );

        // Send the email
        _emailService.SendMessage(message);

        return new Response
        {
            Message = "Confirmation Email sent!"
        };

    }

    public async Task<Response<AuthLoginResponseDto>> LoginAsync(AuthLoginDto model)
    {
        var IPAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(IPAddress))
        {
            throw new Exception("Unable to determine IP address.");
        }
      
        var result=await new AuthLoginDtoValidator().ValidateAsync(model);
        if (!result.IsValid)
        {
           
            throw new ValidationException(result.Errors);
        }
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            
            throw new UnauthorizedException("Email or Password is wrong");
        }
        if (!user.EmailConfirmed)
        {
            throw new UnauthorizedException("Email not confirmed. Please check your inbox to confirm your email address.");
        }

        var isSucceededcheck = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!isSucceededcheck)
        {
          
            throw new UnauthorizedException("Email or Password is wrong");

        }
       
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.Name,user.Email),

        };

        var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
        var token = new JwtSecurityToken(
            claims: claims,
            issuer: _configuration.GetSection("JWT:Issuer").Value,
            audience: _configuration.GetSection("JWT:Audience").Value,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)

            );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        bool isTokenBlacklisted = await _tokenBlacklistService.IsTokenBlacklistedAsync(tokenString);
        if (isTokenBlacklisted)
        {
            throw new UnauthorizedException("Please log in.");
        }
        return new Response<AuthLoginResponseDto>
        {
            Data = new AuthLoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            }
        };



    }
   
  


}