using Microsoft.AspNetCore.Mvc;
using PromptHub.Application.Interfaces;
using PromptHub.Domain.Entities;

namespace PromptHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplateController : ControllerBase
{
    private readonly IPromptTemplateService _templateService;

    public TemplateController(IPromptTemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpGet("public")]
    public async Task<IActionResult> GetPublicTemplates()
    {
        var templates = await _templateService.GetPublicTemplatesAsync();
        return Ok(templates);
    }

    [HttpGet("my-templates")]
    public async Task<IActionResult> GetMyTemplates()
    {
        var userId = Guid.NewGuid(); // Placeholder
        var templates = await _templateService.GetUserTemplatesAsync(userId);
        return Ok(templates);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromBody] PromptTemplate template)
    {
        template.UserId = Guid.NewGuid(); // Placeholder
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
