using Microsoft.SemanticKernel;
using PromptHub.Application.Interfaces;
using PromptHub.Domain.Enums;

namespace PromptHub.Infrastructure.AI;

public class GeminiAiProviderService : IAiProviderService
{
    private readonly Kernel _kernel;

    public GeminiAiProviderService(Kernel kernel)
    {
        _kernel = kernel;
    }

    public async Task<string> GenerateExpandedPromptAsync(string rawInput, RoleType role, string masterInstruction, Dictionary<string, string>? variables = null)
    {
        // Construct the meta-prompt dynamically.
        var promptText = $@"
{masterInstruction}

You will act as a {role}.

Here is the user's raw input task:
""{rawInput}""
";

        if (variables != null && variables.Any())
        {
            var varsText = string.Join("\n", variables.Select(v => $"- {v.Key}: {v.Value}"));
            promptText += $"\nHere are specific variables provided by the user:\n{varsText}";
        }
        
        promptText += "\n\nPlease output ONLY the structured prompt, ready to be copied and pasted by the user into another LLM.";

        // Execute via Semantic Kernel
        var result = await _kernel.InvokePromptAsync(promptText);
        
        return result.ToString();
    }
}
