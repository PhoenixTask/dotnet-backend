using Domain.Tasks;
using FluentValidation;

namespace Application.Tasks.Create;

internal sealed class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.BoardId).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(225);
        RuleFor(x => x.DeadLine).GreaterThanOrEqualTo(DateTime.Now).WithErrorCode(TaskErrors.ExpiredDeadLine.Code).WithMessage(TaskErrors.ExpiredDeadLine.Description);
        RuleFor(x => x.Order).ExclusiveBetween(-100, 100);
        RuleFor(x => x.Priority).ExclusiveBetween(-100, 100);
    }
}
