namespace PromptHub.Application.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(string username, string email, string password);
    Task<string> LoginAsync(string email, string password);
}
