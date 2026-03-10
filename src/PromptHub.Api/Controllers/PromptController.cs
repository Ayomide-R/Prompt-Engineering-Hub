using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromptHub.Application.Interfaces;

namespace PromptHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // Commented out for initial scaffold testing
public class PromptController : ControllerBase
{
    private readonly IPromptService _promptService;

    public PromptController(IPromptService promptService)
    {
        _promptService = promptService;
    }

    [HttpPost("expand")]
    public async Task<IActionResult> ExpandPrompt([FromBody] ExpandPromptRequest request)
    {
        // For scaffold purposes, we generate a fake UserId if auth isn't wired up yet.
        var userId = Guid.NewGuid(); // In reality: Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
        
        var generatedPrompt = await _promptService.ExpandPromptAsync(request.OriginalInput, request.TemplateId, userId);
        
        return Ok(generatedPrompt);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetMyPrompts()
    {
        var userId = Guid.NewGuid(); // Placeholder
        var prompts = await _promptService.GetUserPromptsAsync(userId);
        return Ok(prompts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPrompt(Guid id)
    {
        var prompt = await _promptService.GetPromptByIdAsync(id);
        if (prompt == null) return NotFound();
        return Ok(prompt);
    }

    [HttpPost("{id}/save")]
    public async Task<IActionResult> SavePrompt(Guid id)
    {
        await _promptService.SavePromptAsync(id);
        return Ok();
    }
}

public class ExpandPromptRequest
{
    public string OriginalInput { get; set; } = string.Empty;
    public Guid? TemplateId { get; set; }
}
