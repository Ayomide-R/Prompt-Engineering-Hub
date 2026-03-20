using System.Net.Http.Json;
using PromptHub.Web.Models;

namespace PromptHub.Web.Services;

public class PromptService
{
    private readonly HttpClient _httpClient;

    public PromptService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("PromptHubApi");
    }

    public async Task<List<PromptTemplateDto>> GetPublicTemplatesAsync()
    {
        var response = await _httpClient.GetAsync("api/Template/public");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<PromptTemplateDto>>() ?? new();
        }
        return new();
    }

    public async Task<PromptTemplateDto?> GetTemplateByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<PromptTemplateDto>($"api/Template/{id}");
    }

    public async Task<string> ExpandPromptAsync(ExpandPromptRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Prompt/expand", request);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ExpandPromptResponse>();
            return result?.ExpandedPrompt ?? string.Empty;
        }
        return "Error expanding prompt.";
    }

    public async Task<List<PromptHistoryDto>> GetHistoryAsync()
    {
        var response = await _httpClient.GetAsync("api/Prompt/history");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<PromptHistoryDto>>() ?? new();
        }
        return new();
    }
}

public class PromptHistoryDto
{
    public Guid Id { get; set; }
    public string RawPrompt { get; set; } = string.Empty;
    public string ExpandedPrompt { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
