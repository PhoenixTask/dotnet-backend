using FluentValidation;

namespace Application.Users.Login;
internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(u => u.Username).NotEmpty();
        RuleFor(u => u.Password).NotEmpty();
    }
}
