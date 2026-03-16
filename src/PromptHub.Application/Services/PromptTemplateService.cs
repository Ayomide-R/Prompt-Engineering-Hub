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
        // Archive current version before update
        var currentTemplate = await _context.PromptTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == template.Id);

        if (currentTemplate != null)
        {
            var versionCount = await _context.PromptTemplateVersions
                .CountAsync(v => v.PromptTemplateId == template.Id);

            var version = new PromptTemplateVersion
            {
                PromptTemplateId = currentTemplate.Id,
                Title = currentTemplate.Title,
                Description = currentTemplate.Description,
                Category = currentTemplate.Category,
                DefaultRole = currentTemplate.DefaultRole,
                MasterInstruction = currentTemplate.MasterInstruction,
                RequiredVariables = currentTemplate.RequiredVariables,
                VersionNumber = versionCount + 1,
                CreatedAt = DateTime.UtcNow
            };

            _context.PromptTemplateVersions.Add(version);
        }

        _context.PromptTemplates.Update(template);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<PromptTemplateVersion>> GetTemplateVersionsAsync(Guid templateId)
    {
        return await _context.PromptTemplateVersions
            .Where(v => v.PromptTemplateId == templateId)
            .OrderByDescending(v => v.VersionNumber)
            .ToListAsync();
    }

    public async Task<bool> RevertToVersionAsync(Guid templateId, int versionNumber)
    {
        var template = await _context.PromptTemplates.FindAsync(templateId);
        var version = await _context.PromptTemplateVersions
            .FirstOrDefaultAsync(v => v.PromptTemplateId == templateId && v.VersionNumber == versionNumber);

        if (template == null || version == null) return false;

        // Archive current state before revert
        var versionCount = await _context.PromptTemplateVersions
            .CountAsync(v => v.PromptTemplateId == templateId);

        var currentVersion = new PromptTemplateVersion
        {
            PromptTemplateId = template.Id,
            Title = template.Title,
            Description = template.Description,
            Category = template.Category,
            DefaultRole = template.DefaultRole,
            MasterInstruction = template.MasterInstruction,
            RequiredVariables = template.RequiredVariables,
            VersionNumber = versionCount + 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.PromptTemplateVersions.Add(currentVersion);

        // Revert template state
        template.Title = version.Title;
        template.Description = version.Description;
        template.Category = version.Category;
        template.DefaultRole = version.DefaultRole;
        template.MasterInstruction = version.MasterInstruction;
        template.RequiredVariables = version.RequiredVariables;

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
