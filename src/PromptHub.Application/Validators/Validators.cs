using FluentValidation;
using PromptHub.Application.DTOs.Templates;
using PromptHub.Application.DTOs.Prompts;
using PromptHub.Application.DTOs.Auth;

namespace PromptHub.Application.Validators;

public class CreateTemplateValidator : AbstractValidator<CreateTemplateRequest>
{
    public CreateTemplateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(50);
        RuleFor(x => x.MasterInstruction).NotEmpty();
    }
}

public class UpdateTemplateValidator : AbstractValidator<UpdateTemplateRequest>
{
    public UpdateTemplateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(50);
        RuleFor(x => x.MasterInstruction).NotEmpty();
    }
}

public class ExpandPromptValidator : AbstractValidator<ExpandPromptRequest>
{
    public ExpandPromptValidator()
    {
        RuleFor(x => x.OriginalInput).NotEmpty().MinimumLength(10);
    }
}

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
    }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
