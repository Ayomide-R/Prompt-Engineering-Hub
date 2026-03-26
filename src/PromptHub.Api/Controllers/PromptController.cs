using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromptHub.Application.Interfaces;
using PromptHub.Application.DTOs.Prompts;
using FluentValidation;
using System.Security.Claims;

namespace PromptHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("fixed")]
public class PromptController : ControllerBase
{
    private readonly IPromptService _promptService;
    private readonly IValidator<ExpandPromptRequest> _expandValidator;
    private readonly ILogger<PromptController> _logger;

    public PromptController(
        IPromptService promptService, 
        IValidator<ExpandPromptRequest> expandValidator,
        ILogger<PromptController> logger)
    {
        _promptService = promptService;
        _expandValidator = expandValidator;
        _logger = logger;
    }

    [HttpPost("expand")]
    public async Task<IActionResult> ExpandPrompt([FromBody] ExpandPromptRequest request)
    {
        var validationResult = await _expandValidator.ValidateAsync(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();
        
        var generatedPrompt = await _promptService.ExpandPromptAsync(request.OriginalInput, request.TemplateId, userId, request.Provider, request.Role);
        
        _logger.LogInformation("Prompt expanded for User {UserId} using Template {TemplateId}", userId, request.TemplateId ?? Guid.Empty);

        var response = new PromptResponse(
            generatedPrompt.Id, 
            generatedPrompt.OriginalInput, 
            generatedPrompt.FinalPrompt, 
            generatedPrompt.UsedRole, 
            generatedPrompt.UsedProvider,
            generatedPrompt.GeneratedAt, 
            generatedPrompt.IsSaved, 
            generatedPrompt.PromptTemplateId);

        return Ok(response);
    }

    [HttpPost("compare")]
    public async Task<IActionResult> ComparePrompts([FromBody] ComparePromptsRequest request)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var results = await _promptService.ExpandPromptMultiAsync(request.OriginalInput, request.TemplateId, userId, request.Providers, null); // Roles not supported in Compare yet in API request DTO

        var response = results.Select(p => new PromptResponse(
            p.Id, p.OriginalInput, p.FinalPrompt, p.UsedRole, p.UsedProvider, p.GeneratedAt, p.IsSaved, p.PromptTemplateId));

        return Ok(response);
    }

    [HttpPost("batch")]
    public async Task<IActionResult> BatchExpand([FromBody] BatchPromptRequest request)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var results = await _promptService.ExpandPromptBatchAsync(request.OriginalInputs, request.TemplateId, userId, request.Provider, request.Role);

        var response = results.Select(p => new PromptResponse(
            p.Id, p.OriginalInput, p.FinalPrompt, p.UsedRole, p.UsedProvider, p.GeneratedAt, p.IsSaved, p.PromptTemplateId));

        return Ok(response);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetMyPrompts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var pagedPrompts = await _promptService.GetUserPromptsAsync(userId, pageNumber, pageSize);
        
        var dtos = pagedPrompts.Items.Select(p => new PromptResponse(
            p.Id, p.OriginalInput, p.FinalPrompt, p.UsedRole, p.UsedProvider, p.GeneratedAt, p.IsSaved, p.PromptTemplateId));
            
        var response = new PromptHub.Application.DTOs.Common.PagedResponse<PromptResponse>(
            dtos, pagedPrompts.TotalCount, pagedPrompts.PageNumber, pagedPrompts.PageSize);
            
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPrompt(Guid id)
    {
        var prompt = await _promptService.GetPromptByIdAsync(id);
        if (prompt == null) return NotFound();

        var response = new PromptResponse(
            prompt.Id, prompt.OriginalInput, prompt.FinalPrompt, prompt.UsedRole, prompt.UsedProvider, prompt.GeneratedAt, prompt.IsSaved, prompt.PromptTemplateId);

        return Ok(response);
    }

    [HttpPost("{id}/save")]
    public async Task<IActionResult> SavePrompt(Guid id)
    {
        await _promptService.SavePromptAsync(id);
        return Ok();
    }
}
