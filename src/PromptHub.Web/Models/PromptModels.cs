namespace PromptHub.Web.Models;

public class PromptTemplateDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ExpandPromptRequest
{
    public string Prompt { get; set; } = string.Empty;
    public Dictionary<string, string> Variables { get; set; } = new();
    public string Provider { get; set; } = "Gemini";
}

public class ExpandPromptResponse
{
    public string ExpandedPrompt { get; set; } = string.Empty;
}
