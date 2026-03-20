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
    public string OriginalInput { get; set; } = string.Empty;
    public Guid? TemplateId { get; set; }
    public string Provider { get; set; } = "Gemini";
}

public class ComparePromptsRequest
{
    public string OriginalInput { get; set; } = string.Empty;
    public Guid? TemplateId { get; set; }
    public List<string> Providers { get; set; } = new();
}

public class BatchPromptRequest
{
    public List<string> OriginalInputs { get; set; } = new();
    public Guid? TemplateId { get; set; }
    public string? Provider { get; set; } = "Gemini";
}

public class PromptResponse
{
    public Guid Id { get; set; }
    public string OriginalInput { get; set; } = string.Empty;
    public string FinalPrompt { get; set; } = string.Empty;
    public string UsedRole { get; set; } = string.Empty;
    public string? UsedProvider { get; set; }
    public DateTime GeneratedAt { get; set; }
    public bool IsSaved { get; set; }
    public Guid? TemplateId { get; set; }
}
