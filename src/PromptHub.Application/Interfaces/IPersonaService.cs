using PromptHub.Domain.Entities;
using PromptHub.Domain.Enums;

namespace PromptHub.Application.Interfaces;

public interface IPersonaService
{
    Task<IEnumerable<GlobalPersona>> GetAllPersonasAsync();
    Task<GlobalPersona?> GetPersonaByRoleAsync(RoleType role);
    Task<GlobalPersona> UpsertPersonaAsync(GlobalPersona persona);
    Task<bool> DeletePersonaAsync(Guid id);
}
