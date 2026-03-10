using Microsoft.EntityFrameworkCore;
using PromptHub.Domain.Entities;

namespace PromptHub.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<PromptTemplate> PromptTemplates { get; }
    DbSet<GeneratedPrompt> GeneratedPrompts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
