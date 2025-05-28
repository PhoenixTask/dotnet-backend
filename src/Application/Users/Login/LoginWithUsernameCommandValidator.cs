using FluentValidation;

namespace Application.Users.Login;
internal sealed class LoginWithUsernameCommandValidator : AbstractValidator <LoginWithUsernameCommand>
{
    public LoginWithUsernameCommandValidator()
    {
        RuleFor(u=>u.Username).NotEmpty();
        RuleFor(u=>u.Password).NotEmpty();
    }
}
