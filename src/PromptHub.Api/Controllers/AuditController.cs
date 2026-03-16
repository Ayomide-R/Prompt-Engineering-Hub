using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PromptHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class AuditController : ControllerBase
{
    private readonly ILogger<AuditController> _logger;

    public AuditController(ILogger<AuditController> logger)
    {
        _logger = logger;
    }

    [HttpGet("logs")]
    public IActionResult GetLogs()
    {
        // In a real app, this would read from a database or file.
        // For now, we'll return a placeholder to demonstrate Admin-only access.
        _logger.LogInformation("Admin requested audit logs.");
        
        return Ok(new[] 
        { 
            new { Timestamp = DateTime.UtcNow.AddMinutes(-10), Action = "Template Created", User = "admin@example.com" },
            new { Timestamp = DateTime.UtcNow.AddMinutes(-5), Action = "Prompt Expanded", User = "user@example.com" }
        });
    }
}
