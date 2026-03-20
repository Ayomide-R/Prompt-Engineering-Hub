using PromptHub.Domain.Enums;

namespace PromptHub.Domain.Entities;

public class GlobalPersona
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public RoleType Role { get; set; }
    public string MasterInstruction { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
