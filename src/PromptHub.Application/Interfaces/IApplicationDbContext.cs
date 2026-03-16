using Microsoft.EntityFrameworkCore;
using PromptHub.Domain.Entities;

namespace PromptHub.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<PromptTemplate> PromptTemplates { get; set; }
    DbSet<GeneratedPrompt> GeneratedPrompts { get; set; }
    DbSet<PromptTemplateVersion> PromptTemplateVersions { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
