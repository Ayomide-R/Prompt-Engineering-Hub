namespace PromptHub.Application.Options;

public class AIProviderOptions
{
    public const string SectionName = "AIProvider";
    
    public GeminiOptions Gemini { get; set; } = new();
    public OpenAIOptions OpenAI { get; set; } = new();
    public AnthropicOptions Anthropic { get; set; } = new();
    
    public string DefaultProvider { get; set; } = "Gemini";
}

public class GeminiOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string ModelId { get; set; } = "gemini-2.5-flash";
}

public class OpenAIOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string ModelId { get; set; } = "gpt-4";
}

public class AnthropicOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string ModelId { get; set; } = "claude-3-opus-20240229";
}
