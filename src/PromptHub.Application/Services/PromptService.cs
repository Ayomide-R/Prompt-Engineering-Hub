using Microsoft.EntityFrameworkCore;
using PromptHub.Application.DTOs.Common;
using PromptHub.Application.Interfaces;
using PromptHub.Domain.Entities;

namespace PromptHub.Application.Services;

public class PromptService : IPromptService
{
    private readonly IApplicationDbContext _context;
    private readonly IAiProviderService _aiProvider;
    private readonly IPersonaService _personaService;

    public PromptService(IApplicationDbContext context, IAiProviderService aiProvider, IPersonaService personaService)
    {
        _context = context;
        _aiProvider = aiProvider;
        _personaService = personaService;
    }

    public async Task<GeneratedPrompt> ExpandPromptAsync(string originalInput, Guid? templateId, Guid userId, string? provider = null)
    {
        var role = await GetRoleAndInstructionAsync(templateId);
        return await ExecuteExpansionAsync(originalInput, role.Role, role.Instruction, templateId, userId, provider);
    }

    public async Task<List<GeneratedPrompt>> ExpandPromptMultiAsync(string originalInput, Guid? templateId, Guid userId, List<string> providers)
    {
        var role = await GetRoleAndInstructionAsync(templateId);
        var tasks = providers.Select(p => ExecuteExpansionAsync(originalInput, role.Role, role.Instruction, templateId, userId, p));
        var results = await Task.WhenAll(tasks);
        return results.ToList();
    }

    public async Task<List<GeneratedPrompt>> ExpandPromptBatchAsync(List<string> inputs, Guid? templateId, Guid userId, string? provider = null)
    {
        var role = await GetRoleAndInstructionAsync(templateId);
        var tasks = inputs.Select(input => ExecuteExpansionAsync(input, role.Role, role.Instruction, templateId, userId, provider));
        var results = await Task.WhenAll(tasks);
        return results.ToList();
    }

    private async Task<(Domain.Enums.RoleType Role, string Instruction)> GetRoleAndInstructionAsync(Guid? templateId)
    {
        var role = Domain.Enums.RoleType.GeneralAssistant;
        
        if (templateId.HasValue)
        {
            var template = await _context.PromptTemplates.FindAsync(templateId.Value);
            if (template != null)
            {
                var templateInstruction = template.MasterInstruction;
                if (!string.IsNullOrWhiteSpace(templateInstruction))
                {
                    return (template.DefaultRole, templateInstruction);
                }
                role = template.DefaultRole;
            }
        }

        var persona = await _personaService.GetPersonaByRoleAsync(role);
        var masterInstruction = persona?.MasterInstruction 
            ?? "You are an expert Prompt Engineer. When I give you a task, rewrite it into a detailed, structured prompt that includes Context, Task, Constraints, and Output Format.";

        return (role, masterInstruction);
    }

    private async Task<GeneratedPrompt> ExecuteExpansionAsync(string originalInput, Domain.Enums.RoleType role, string masterInstruction, Guid? templateId, Guid userId, string? provider)
    {
        // Call the AI Provider to expand the prompt
        var expandedPromptText = await _aiProvider.GenerateExpandedPromptAsync(originalInput, role, masterInstruction, provider);

        var generatedPrompt = new GeneratedPrompt
        {
            OriginalInput = originalInput,
            FinalPrompt = expandedPromptText,
            UsedRole = role,
            UsedProvider = provider ?? "Gemini",
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

    public async Task<PagedResponse<GeneratedPrompt>> GetUserPromptsAsync(Guid userId, int pageNumber, int pageSize)
    {
        var query = _context.GeneratedPrompts
            .Where(gp => gp.UserId == userId)
            .OrderByDescending(gp => gp.GeneratedAt);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<GeneratedPrompt>(items, totalCount, pageNumber, pageSize);
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
