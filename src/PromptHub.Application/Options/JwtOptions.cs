namespace PromptHub.Application.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string Secret { get; init; } = null!;
    public int ExpiryMinutes { get; init; }
}
