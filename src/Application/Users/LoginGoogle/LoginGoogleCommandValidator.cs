using FluentValidation;

namespace Application.Users.LoginGoogle;

internal sealed class LoginGoogleCommandValidator : AbstractValidator<LoginGoogleCommand>
{
    public LoginGoogleCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}
