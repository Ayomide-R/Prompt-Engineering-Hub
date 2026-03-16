using PromptHub.Domain.Entities;

namespace PromptHub.Application.Interfaces;

public interface IPromptTemplateService
{
    Task<IEnumerable<PromptTemplate>> GetPublicTemplatesAsync();
    Task<IEnumerable<PromptTemplate>> GetUserTemplatesAsync(Guid userId);
    Task<PromptTemplate> CreateTemplateAsync(PromptTemplate template);
    Task<PromptTemplate?> GetTemplateByIdAsync(Guid id);
    Task<bool> UpdateTemplateAsync(PromptTemplate template);
    Task<bool> DeleteTemplateAsync(Guid id);
    Task<IEnumerable<PromptTemplate>> SearchTemplatesAsync(string? searchTerm, string? category);
    Task<IEnumerable<PromptTemplateVersion>> GetTemplateVersionsAsync(Guid templateId);
    Task<bool> RevertToVersionAsync(Guid templateId, int versionNumber);
}
