using FluentValidation;

namespace Application.Tasks.ChangeOrder;

internal sealed class ChangeTasksOrderCommandValidator : AbstractValidator<ChangeTasksOrderCommand>
{
    public ChangeTasksOrderCommandValidator()
    {
        RuleFor(x => x.Tasks)
             .NotEmpty();

        RuleForEach(x => x.Tasks)
            .ChildRules(task =>
            {
                task.RuleFor(t => t.Order)
                     .GreaterThanOrEqualTo(1);

                task.RuleFor(t => t.Id)
                     .NotEmpty();
            });
    }
}
