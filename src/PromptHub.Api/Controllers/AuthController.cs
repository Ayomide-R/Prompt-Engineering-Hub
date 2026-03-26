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
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService, 
        IValidator<RegisterRequest> registerValidator, 
        IValidator<LoginRequest> loginValidator,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var validationResult = await _registerValidator.ValidateAsync(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

        var response = await _authService.RegisterAsync(request.Username, request.Email, request.Password);
        
        _logger.LogInformation("User registered: {Username} ({Email})", request.Username, request.Email);

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

        var response = await _authService.LoginAsync(request.Email, request.Password);
        
        _logger.LogInformation("User logged in: {Email}", request.Email);

        return Ok(response);
    }
}
