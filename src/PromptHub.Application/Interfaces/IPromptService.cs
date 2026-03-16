using PromptHub.Application.DTOs.Common;
using PromptHub.Domain.Entities;

namespace PromptHub.Application.Interfaces;

public interface IPromptService
{
    Task<GeneratedPrompt> ExpandPromptAsync(string originalInput, Guid? templateId, Guid userId, string? provider = null);
    Task<PagedResponse<GeneratedPrompt>> GetUserPromptsAsync(Guid userId, int pageNumber, int pageSize);
    Task<GeneratedPrompt?> GetPromptByIdAsync(Guid id);
    Task SavePromptAsync(Guid promptId);
}
