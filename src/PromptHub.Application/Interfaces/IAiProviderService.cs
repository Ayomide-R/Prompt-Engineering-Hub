using PromptHub.Domain.Enums;

namespace PromptHub.Application.Interfaces;

public interface IAiProviderService
{
    /// <summary>
    /// Calls the underlying AI provider (e.g. Gemini) to expand a raw input into a full structured prompt based on master instructions.
    /// </summary>
    Task<string> GenerateExpandedPromptAsync(string rawInput, RoleType role, string masterInstruction, Dictionary<string, string>? variables = null);
}
