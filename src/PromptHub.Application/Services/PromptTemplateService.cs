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

    public async Task<bool> UpdateTemplateAsync(PromptTemplate template)
    {
        _context.PromptTemplates.Update(template);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteTemplateAsync(Guid id)
    {
        var template = await _context.PromptTemplates.FindAsync(id);
        if (template == null) return false;

        _context.PromptTemplates.Remove(template);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<PromptTemplate>> SearchTemplatesAsync(string? searchTerm, string? category)
    {
        var query = _context.PromptTemplates
            .Where(t => t.IsPublic)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(t => 
                t.Title.ToLower().Contains(term) || 
                t.Description.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(t => t.Category == category);
        }

        return await query
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
}
