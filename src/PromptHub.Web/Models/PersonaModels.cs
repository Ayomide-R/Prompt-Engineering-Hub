namespace PromptHub.Web.Models;

public enum RoleType
{
    GeneralAssistant,
    SeniorDeveloper,
    CreativeWriter,
    LegalExpert,
    AcademicResearcher,
    MarketingSpecialist,
    DataAnalyst
}

public class GlobalPersonaDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public RoleType Role { get; set; }
    public string MasterInstruction { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}
