using PromptHub.Application.DTOs.Common;
using PromptHub.Domain.Entities;
using PromptHub.Domain.Enums;

namespace PromptHub.Application.Interfaces;

public interface IPromptService
{
    Task<GeneratedPrompt> ExpandPromptAsync(string originalInput, Guid? templateId, Guid userId, string? provider = null, RoleType? requestedRole = null);
    Task<List<GeneratedPrompt>> ExpandPromptMultiAsync(string originalInput, Guid? templateId, Guid userId, List<string> providers, RoleType? requestedRole = null);
    Task<List<GeneratedPrompt>> ExpandPromptBatchAsync(List<string> inputs, Guid? templateId, Guid userId, string? provider = null, RoleType? requestedRole = null);
    Task<PagedResponse<GeneratedPrompt>> GetUserPromptsAsync(Guid userId, int pageNumber, int pageSize);
    Task<GeneratedPrompt?> GetPromptByIdAsync(Guid id);
    Task SavePromptAsync(Guid promptId);
}
