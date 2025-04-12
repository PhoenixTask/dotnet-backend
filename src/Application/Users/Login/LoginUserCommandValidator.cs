using FluentValidation;

namespace Application.Users.Login;
internal sealed class LoginUserCommandValidator : AbstractValidator <LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(u=>u.Username).NotEmpty();
        RuleFor(u=>u.Password).NotEmpty();
    }
}
