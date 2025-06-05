using FluentValidation;

namespace Application.Boards.ChangeOrder;

internal sealed class ChangeBoardOrderCommandValidator : AbstractValidator<ChangeBoardOrderCommand>
{
    public ChangeBoardOrderCommandValidator()
    {
        RuleFor(x => x.Order).ExclusiveBetween(int.MinValue, int.MaxValue);
    }
}
