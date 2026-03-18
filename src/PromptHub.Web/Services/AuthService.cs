using System.Net.Http.Json;
using Microsoft.JSInterop;
using PromptHub.Web.Models;
using PromptHub.Web.Providers;

namespace PromptHub.Web.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly CustomAuthStateProvider _authStateProvider;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime, CustomAuthStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/login", request);
        
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (result != null && !string.IsNullOrEmpty(result.Token))
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", result.Token);
                _authStateProvider.NotifyUserAuthentication(result.Token);
                return true;
            }
        }
        return false;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/register", request);
        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        _authStateProvider.NotifyUserLogout();
    }
}
