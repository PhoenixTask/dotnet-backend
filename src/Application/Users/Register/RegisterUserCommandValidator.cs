using FluentValidation;

namespace Application.Users.Register;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.Username).NotEmpty();
        RuleFor(c => c.Password).NotEmpty().MinimumLength(8);
    }
}
