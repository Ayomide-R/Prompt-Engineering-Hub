using Microsoft.EntityFrameworkCore;
using PromptHub.Application.Interfaces;
using PromptHub.Domain.Entities;

namespace PromptHub.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<PromptTemplate> PromptTemplates { get; set; } = null!;
    public DbSet<GeneratedPrompt> GeneratedPrompts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Ensure email uniqueness
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
            
        // Configure relationships
        modelBuilder.Entity<PromptTemplate>()
            .HasOne(pt => pt.User)
            .WithMany(u => u.Templates)
            .HasForeignKey(pt => pt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GeneratedPrompt>()
            .HasOne(gp => gp.User)
            .WithMany(u => u.GeneratedPrompts)
            .HasForeignKey(gp => gp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<GeneratedPrompt>()
            .HasOne(gp => gp.PromptTemplate)
            .WithMany(pt => pt.GeneratedPrompts)
            .HasForeignKey(gp => gp.PromptTemplateId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
