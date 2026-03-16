using PromptHub.Domain.Enums;

namespace PromptHub.Application.Interfaces;

public interface IAiProviderService
{
    /// <summary>
    /// Calls the underlying AI provider to expand a raw input into a full structured prompt based on master instructions.
    /// </summary>
    Task<string> GenerateExpandedPromptAsync(string rawInput, RoleType role, string masterInstruction, string? provider = null, Dictionary<string, string>? variables = null);
}
