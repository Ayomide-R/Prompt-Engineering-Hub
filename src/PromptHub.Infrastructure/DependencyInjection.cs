#pragma warning disable SKEXP0070 // Ollama
#pragma warning disable SKEXP0011 // Various
#pragma warning disable SKEXP0001 // Core

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using PromptHub.Application.Interfaces;
using PromptHub.Application.Services;
using PromptHub.Infrastructure.AI;
using PromptHub.Infrastructure.Authentication;
using PromptHub.Infrastructure.Data;
using PromptHub.Application.Options;
using PromptHub.Infrastructure.Services;

namespace PromptHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
            
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // Multi-Model Semantic Kernel Setup
        var aiOptions = configuration.GetSection(AIProviderOptions.SectionName).Get<AIProviderOptions>() ?? new AIProviderOptions();
        services.Configure<AIProviderOptions>(configuration.GetSection(AIProviderOptions.SectionName));

        var builder = Kernel.CreateBuilder();

        // Gemini Integration
        if (!string.IsNullOrEmpty(aiOptions.Gemini.ApiKey))
        {
            builder.AddGoogleAIGeminiChatCompletion(
                modelId: aiOptions.Gemini.ModelId, 
                apiKey: aiOptions.Gemini.ApiKey,
                serviceId: "Gemini");
        }

        // OpenAI Integration
        if (!string.IsNullOrEmpty(aiOptions.OpenAI.ApiKey))
        {
            builder.AddOpenAIChatCompletion(
                modelId: aiOptions.OpenAI.ModelId,
                apiKey: aiOptions.OpenAI.ApiKey,
                serviceId: "OpenAI");
        }

        // Anthropic Integration
        if (!string.IsNullOrEmpty(aiOptions.Anthropic.ApiKey))
        {
            builder.AddAnthropicChatCompletion(
                modelId: aiOptions.Anthropic.ModelId,
                apiKey: aiOptions.Anthropic.ApiKey,
                serviceId: "Anthropic");
        }

        // Ollama Integration
        if (!string.IsNullOrEmpty(aiOptions.Ollama.Endpoint))
        {
            builder.AddOllamaChatCompletion(
                modelId: aiOptions.Ollama.ModelId,
                endpoint: new Uri(aiOptions.Ollama.Endpoint),
                serviceId: "Ollama");
        }

        services.AddSingleton(builder.Build());

        // Domain Services
        services.AddScoped<IPromptService, PromptService>();
        services.AddScoped<IPromptTemplateService, PromptTemplateService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAiProviderService, AiProviderService>();
        services.AddScoped<IPersonaService, PersonaService>();

        // Authentication Services
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
