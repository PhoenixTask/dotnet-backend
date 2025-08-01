using FluentValidation;

namespace Application.Tasks.GetTaskByDate;

internal sealed class GetTaskByDeadLineQueryValidator : AbstractValidator<GetTaskByDeadLineQuery>
{
    public GetTaskByDeadLineQueryValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.StartDate).LessThan(x => x.EndDate);
    }
}
