using PromptHub.Domain.Entities;

namespace PromptHub.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
