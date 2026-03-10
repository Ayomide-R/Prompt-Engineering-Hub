using PromptHub.Domain.Entities;

namespace PromptHub.Application.Interfaces;

public interface IPromptService
{
    Task<GeneratedPrompt> ExpandPromptAsync(string originalInput, Guid? templateId, Guid userId);
    Task<IEnumerable<GeneratedPrompt>> GetUserPromptsAsync(Guid userId);
    Task<GeneratedPrompt?> GetPromptByIdAsync(Guid id);
    Task SavePromptAsync(Guid promptId);
}
