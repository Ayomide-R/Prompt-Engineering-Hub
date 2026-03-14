using Microsoft.AspNetCore.Mvc;
using PromptHub.Application.Interfaces;
using PromptHub.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using PromptHub.Application.DTOs.Templates;
using FluentValidation;

namespace PromptHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("fixed")]
public class TemplateController : ControllerBase
{
    private readonly IPromptTemplateService _templateService;
    private readonly IValidator<CreateTemplateRequest> _createValidator;
    private readonly IValidator<UpdateTemplateRequest> _updateValidator;
    private readonly ILogger<TemplateController> _logger;

    public TemplateController(
        IPromptTemplateService templateService, 
        IValidator<CreateTemplateRequest> createValidator,
        IValidator<UpdateTemplateRequest> updateValidator,
        ILogger<TemplateController> logger)
    {
        _templateService = templateService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    [HttpGet("public")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicTemplates([FromQuery] string? searchTerm, [FromQuery] string? category)
    {
        IEnumerable<PromptTemplate> templates;
        
        if (!string.IsNullOrWhiteSpace(searchTerm) || !string.IsNullOrWhiteSpace(category))
        {
            templates = await _templateService.SearchTemplatesAsync(searchTerm, category);
        }
        else
        {
            templates = await _templateService.GetPublicTemplatesAsync();
        }

        var response = templates.Select(t => new TemplateResponse(
            t.Id, t.Title, t.Description, t.Category, t.DefaultRole, t.MasterInstruction, t.RequiredVariables, t.IsPublic, t.CreatedAt, t.UserId));
        return Ok(response);
    }

    [HttpGet("my-templates")]
    public async Task<IActionResult> GetMyTemplates()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var templates = await _templateService.GetUserTemplatesAsync(userId);
        var response = templates.Select(t => new TemplateResponse(
            t.Id, t.Title, t.Description, t.Category, t.DefaultRole, t.MasterInstruction, t.RequiredVariables, t.IsPublic, t.CreatedAt, t.UserId));
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateTemplateRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var template = new PromptTemplate
        {
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            DefaultRole = request.DefaultRole,
            MasterInstruction = request.MasterInstruction,
            RequiredVariables = request.RequiredVariables,
            IsPublic = request.IsPublic,
            UserId = userId
        };

        var created = await _templateService.CreateTemplateAsync(template);
        
        _logger.LogInformation("Template created: {TemplateTitle} by User {UserId}", created.Title, userId);

        var response = new TemplateResponse(
            created.Id, created.Title, created.Description, created.Category, created.DefaultRole, created.MasterInstruction, created.RequiredVariables, created.IsPublic, created.CreatedAt, created.UserId);
        
        return CreatedAtAction(nameof(GetTemplate), new { id = created.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplate(Guid id)
    {
        var template = await _templateService.GetTemplateByIdAsync(id);
        if (template == null) return NotFound();

        var response = new TemplateResponse(
            template.Id, template.Title, template.Description, template.Category, template.DefaultRole, template.MasterInstruction, template.RequiredVariables, template.IsPublic, template.CreatedAt, template.UserId);
        
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTemplate(Guid id, [FromBody] UpdateTemplateRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var template = await _templateService.GetTemplateByIdAsync(id);
        if (template == null) return NotFound();

        if (template.UserId != userId)
        {
            _logger.LogWarning("User {UserId} attempted to update template {TemplateId} owned by {OwnerId}", userId, id, template.UserId);
            return Forbid();
        }

        template.Title = request.Title;
        template.Description = request.Description;
        template.Category = request.Category;
        template.DefaultRole = request.DefaultRole;
        template.MasterInstruction = request.MasterInstruction;
        template.RequiredVariables = request.RequiredVariables;
        template.IsPublic = request.IsPublic;

        var updated = await _templateService.UpdateTemplateAsync(template);
        if (!updated) return StatusCode(500, "An error occurred while updating the template.");

        _logger.LogInformation("Template {TemplateId} updated by User {UserId}", id, userId);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate(Guid id)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var template = await _templateService.GetTemplateByIdAsync(id);
        if (template == null) return NotFound();

        if (template.UserId != userId)
        {
            _logger.LogWarning("User {UserId} attempted to delete template {TemplateId} owned by {OwnerId}", userId, id, template.UserId);
            return Forbid();
        }

        var deleted = await _templateService.DeleteTemplateAsync(id);
        if (!deleted) return StatusCode(500, "An error occurred while deleting the template.");

        _logger.LogInformation("Template {TemplateId} deleted by User {UserId}", id, userId);

        return NoContent();
    }
}
