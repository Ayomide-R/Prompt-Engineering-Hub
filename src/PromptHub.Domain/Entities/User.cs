namespace PromptHub.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<PromptTemplate> Templates { get; set; } = new List<PromptTemplate>();
    public ICollection<GeneratedPrompt> GeneratedPrompts { get; set; } = new List<GeneratedPrompt>();
}
