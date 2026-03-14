using Microsoft.AspNetCore.Mvc;
using PromptHub.Application.DTOs.Auth;
using PromptHub.Application.Interfaces;
using FluentValidation;

namespace PromptHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<LoginRequest> _loginValidator;

    public AuthController(IAuthService authService, IValidator<RegisterRequest> registerValidator, IValidator<LoginRequest> loginValidator)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var validationResult = await _registerValidator.ValidateAsync(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

        try
        {
            var token = await _authService.RegisterAsync(request.Username, request.Email, request.Password);
            return Ok(new AuthResponse(token));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

        try
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            return Ok(new AuthResponse(token));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }
}
