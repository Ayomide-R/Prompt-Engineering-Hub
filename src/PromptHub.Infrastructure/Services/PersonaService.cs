using Microsoft.EntityFrameworkCore;
using PromptHub.Application.Interfaces;
using PromptHub.Domain.Entities;
using PromptHub.Domain.Enums;

namespace PromptHub.Infrastructure.Services;

public class PersonaService : IPersonaService
{
    private readonly IApplicationDbContext _context;

    public PersonaService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GlobalPersona>> GetAllPersonasAsync()
    {
        return await _context.GlobalPersonas.ToListAsync();
    }

    public async Task<GlobalPersona?> GetPersonaByRoleAsync(RoleType role)
    {
        return await _context.GlobalPersonas.FirstOrDefaultAsync(p => p.Role == role);
    }

    public async Task<GlobalPersona> UpsertPersonaAsync(GlobalPersona persona)
    {
        var existing = await _context.GlobalPersonas.FirstOrDefaultAsync(p => p.Role == persona.Role);
        if (existing != null)
        {
            existing.MasterInstruction = persona.MasterInstruction;
            existing.Description = persona.Description;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.GlobalPersonas.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        _context.GlobalPersonas.Add(persona);
        await _context.SaveChangesAsync();
        return persona;
    }

    public async Task<bool> DeletePersonaAsync(Guid id)
    {
        var persona = await _context.GlobalPersonas.FindAsync(id);
        if (persona == null) return false;

        _context.GlobalPersonas.Remove(persona);
        await _context.SaveChangesAsync();
        return true;
    }
}
