using FluentValidation;


namespace Application.Tasks.ChangeOrder;

internal sealed class ChangeTaskOrderCommandValidator : AbstractValidator<ChangeTaskOrderCommand>
{
    public ChangeTaskOrderCommandValidator()
    {
        RuleFor(x => x.Order).ExclusiveBetween(int.MinValue, int.MaxValue);
    }
}
