using Microsoft.EntityFrameworkCore;
using PromptHub.Application.Interfaces;
using PromptHub.Domain.Entities;

namespace PromptHub.Application.Services;

public class PromptTemplateService : IPromptTemplateService
{
    private readonly IApplicationDbContext _context;

    public PromptTemplateService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PromptTemplate> CreateTemplateAsync(PromptTemplate template)
    {
        _context.PromptTemplates.Add(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task<IEnumerable<PromptTemplate>> GetPublicTemplatesAsync()
    {
        return await _context.PromptTemplates
            .Where(t => t.IsPublic)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<PromptTemplate?> GetTemplateByIdAsync(Guid id)
    {
        return await _context.PromptTemplates.FindAsync(id);
    }

    public async Task<IEnumerable<PromptTemplate>> GetUserTemplatesAsync(Guid userId)
    {
        return await _context.PromptTemplates
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
}
