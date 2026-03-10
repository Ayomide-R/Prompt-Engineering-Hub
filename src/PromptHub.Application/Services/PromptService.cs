using Microsoft.EntityFrameworkCore;
using PromptHub.Application.Interfaces;
using PromptHub.Domain.Entities;

namespace PromptHub.Application.Services;

public class PromptService : IPromptService
{
    private readonly IApplicationDbContext _context;
    private readonly IAiProviderService _aiProvider;

    public PromptService(IApplicationDbContext context, IAiProviderService aiProvider)
    {
        _context = context;
        _aiProvider = aiProvider;
    }

    public async Task<GeneratedPrompt> ExpandPromptAsync(string originalInput, Guid? templateId, Guid userId)
    {
        var role = Domain.Enums.RoleType.GeneralAssistant;
        var masterInstruction = "You are an expert Prompt Engineer. When I give you a task, rewrite it into a detailed, structured prompt that includes Context, Task, Constraints, and Output Format.";
        
        if (templateId.HasValue)
        {
            var template = await _context.PromptTemplates.FindAsync(templateId.Value);
            if (template != null)
            {
                role = template.DefaultRole;
                masterInstruction = template.MasterInstruction;
            }
        }

        // Call the AI Provider to expand the prompt
        var expandedPromptText = await _aiProvider.GenerateExpandedPromptAsync(originalInput, role, masterInstruction);

        var generatedPrompt = new GeneratedPrompt
        {
            OriginalInput = originalInput,
            FinalPrompt = expandedPromptText,
            UsedRole = role,
            UserId = userId,
            PromptTemplateId = templateId,
            IsSaved = false
        };

        _context.GeneratedPrompts.Add(generatedPrompt);
        await _context.SaveChangesAsync();

        return generatedPrompt;
    }

    public async Task<GeneratedPrompt?> GetPromptByIdAsync(Guid id)
    {
        return await _context.GeneratedPrompts.FindAsync(id);
    }

    public async Task<IEnumerable<GeneratedPrompt>> GetUserPromptsAsync(Guid userId)
    {
        return await _context.GeneratedPrompts
            .Where(gp => gp.UserId == userId)
            .OrderByDescending(gp => gp.GeneratedAt)
            .ToListAsync();
    }

    public async Task SavePromptAsync(Guid promptId)
    {
        var prompt = await _context.GeneratedPrompts.FindAsync(promptId);
        if (prompt != null)
        {
            prompt.IsSaved = true;
            await _context.SaveChangesAsync();
        }
    }
}
