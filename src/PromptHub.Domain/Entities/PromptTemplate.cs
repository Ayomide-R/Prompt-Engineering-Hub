using PromptHub.Domain.Enums;

namespace PromptHub.Domain.Entities;

public class PromptTemplate
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    
    public RoleType DefaultRole { get; set; } = RoleType.GeneralAssistant;
    
    // The master instruction base
    public string MasterInstruction { get; set; } = string.Empty;

    // JSON string array or comma separated variables needed for this template
    public string RequiredVariables { get; set; } = string.Empty;

    public bool IsPublic { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    public ICollection<GeneratedPrompt> GeneratedPrompts { get; set; } = new List<GeneratedPrompt>();
}
