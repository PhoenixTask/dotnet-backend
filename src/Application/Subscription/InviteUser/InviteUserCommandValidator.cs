using FluentValidation;

namespace Application.Subscription.InviteUser;

internal sealed class InviteUserCommandValidator : AbstractValidator<InviteUserCommand>
{
    public InviteUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}
