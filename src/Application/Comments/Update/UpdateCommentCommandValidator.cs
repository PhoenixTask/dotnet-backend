using FluentValidation;

namespace Application.Comments.Update;

internal sealed class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(x => x.Content).NotEmpty();
    }
}
