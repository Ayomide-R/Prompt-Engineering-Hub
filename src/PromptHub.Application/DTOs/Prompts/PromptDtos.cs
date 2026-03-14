using PromptHub.Domain.Enums;

namespace PromptHub.Application.DTOs.Prompts;

public record ExpandPromptRequest(string OriginalInput, Guid? TemplateId);

public record PromptResponse(
    Guid Id, 
    string OriginalInput, 
    string FinalPrompt, 
    RoleType UsedRole, 
    DateTime GeneratedAt, 
    bool IsSaved, 
    Guid? TemplateId);
