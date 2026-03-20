using PromptHub.Domain.Enums;

namespace PromptHub.Application.DTOs.Prompts;

/// <summary>
/// Request to expand a prompt.
/// </summary>
/// <param name="OriginalInput">The raw user input.</param>
/// <param name="TemplateId">Optional template ID to use.</param>
/// <param name="Provider">Optional AI Provider: "Gemini", "OpenAI", "Anthropic", or "Ollama". Defaults to Gemini.</param>
public record ExpandPromptRequest(string OriginalInput, Guid? TemplateId, string? Provider = null);

public record ComparePromptsRequest(string OriginalInput, Guid? TemplateId, List<string> Providers);

public record BatchPromptRequest(List<string> OriginalInputs, Guid? TemplateId, string? Provider = null);

public record PromptResponse(
    Guid Id, 
    string OriginalInput, 
    string FinalPrompt, 
    RoleType UsedRole, 
    string? UsedProvider,
    DateTime GeneratedAt, 
    bool IsSaved, 
    Guid? TemplateId);
