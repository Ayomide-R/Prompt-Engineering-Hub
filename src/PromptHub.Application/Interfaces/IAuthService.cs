using PromptHub.Application.DTOs.Auth;

namespace PromptHub.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(string username, string email, string password);
    Task<AuthResponse> LoginAsync(string email, string password);
}
