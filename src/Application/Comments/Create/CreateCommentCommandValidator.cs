using FluentValidation;

namespace Application.Comments.Create;

internal sealed class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.Content).NotEmpty();
    }
}
