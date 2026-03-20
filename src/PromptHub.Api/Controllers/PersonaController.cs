using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromptHub.Application.Interfaces;
using PromptHub.Domain.Entities;
using PromptHub.Domain.Enums;

namespace PromptHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class PersonaController : ControllerBase
{
    private readonly IPersonaService _personaService;

    public PersonaController(IPersonaService personaService)
    {
        _personaService = personaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var personas = await _personaService.GetAllPersonasAsync();
        return Ok(personas);
    }

    [HttpGet("{role}")]
    public async Task<IActionResult> GetByRole(RoleType role)
    {
        var persona = await _personaService.GetPersonaByRoleAsync(role);
        if (persona == null) return NotFound();
        return Ok(persona);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] GlobalPersona persona)
    {
        var result = await _personaService.UpsertPersonaAsync(persona);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _personaService.DeletePersonaAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
