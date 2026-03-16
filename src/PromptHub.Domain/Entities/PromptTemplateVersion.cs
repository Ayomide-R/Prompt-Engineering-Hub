using PromptHub.Domain.Enums;

namespace PromptHub.Domain.Entities;

public class PromptTemplateVersion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PromptTemplateId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public RoleType DefaultRole { get; set; }
    public string MasterInstruction { get; set; } = string.Empty;
    public string RequiredVariables { get; set; } = string.Empty; // Store as comma-separated or JSON
    public int VersionNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public PromptTemplate PromptTemplate { get; set; } = null!;
}
