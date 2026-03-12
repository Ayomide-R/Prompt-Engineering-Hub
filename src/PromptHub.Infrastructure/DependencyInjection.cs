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

        // Semantic Kernel Setup for Gemini
        var geminiApiKey = configuration["AIProvider:Gemini:ApiKey"];
        var geminiModelId = configuration["AIProvider:Gemini:ModelId"] ?? "gemini-2.5-flash"; // Default to a standard model
        
        var builder = Kernel.CreateBuilder();
        builder.AddGoogleAIGeminiChatCompletion(modelId: geminiModelId, apiKey: geminiApiKey!);
        
        services.AddSingleton(builder.Build());

        // Domain Services
        services.AddScoped<IPromptService, PromptService>();
        services.AddScoped<IPromptTemplateService, PromptTemplateService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAiProviderService, GeminiAiProviderService>();

        // Authentication Services
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
