using PromptHub.Domain.Entities;

namespace PromptHub.Application.Interfaces;

public interface IPromptTemplateService
{
    Task<IEnumerable<PromptTemplate>> GetPublicTemplatesAsync();
    Task<IEnumerable<PromptTemplate>> GetUserTemplatesAsync(Guid userId);
    Task<PromptTemplate> CreateTemplateAsync(PromptTemplate template);
    Task<PromptTemplate?> GetTemplateByIdAsync(Guid id);
}
