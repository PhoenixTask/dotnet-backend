using Domain.Tasks;
using FluentValidation;

namespace Application.Tasks.UpdateDeadLine;

internal sealed class UpdateDeadLineCommandValidator : AbstractValidator<UpdateDeadLineCommand>
{
    public UpdateDeadLineCommandValidator()
    {
        RuleFor(x => x.NewDeadLine).GreaterThanOrEqualTo(DateTime.Now).WithErrorCode(TaskErrors.ExpiredDeadLine.Code).WithMessage(TaskErrors.ExpiredDeadLine.Description).When(x => x.NewDeadLine.HasValue);
    }
}
