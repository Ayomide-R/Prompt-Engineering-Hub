using Microsoft.AspNetCore.Mvc;
using PromptHub.Application.Interfaces;
using PromptHub.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PromptHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TemplateController : ControllerBase
{
    private readonly IPromptTemplateService _templateService;

    public TemplateController(IPromptTemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpGet("public")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicTemplates()
    {
        var templates = await _templateService.GetPublicTemplatesAsync();
        return Ok(templates);
    }

    [HttpGet("my-templates")]
    public async Task<IActionResult> GetMyTemplates()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var templates = await _templateService.GetUserTemplatesAsync(userId);
        return Ok(templates);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromBody] PromptTemplate template)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        template.UserId = userId;
        var created = await _templateService.CreateTemplateAsync(template);
        return CreatedAtAction(nameof(GetTemplate), new { id = created.Id }, created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplate(Guid id)
    {
        var template = await _templateService.GetTemplateByIdAsync(id);
        if (template == null) return NotFound();
        return Ok(template);
    }
}
