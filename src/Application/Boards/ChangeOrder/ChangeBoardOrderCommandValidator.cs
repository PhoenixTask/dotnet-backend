using FluentValidation;

namespace Application.Boards.ChangeOrder;

internal sealed class ChangeBoardOrderCommandValidator : AbstractValidator<ChangeBoardOrderCommand>
{
    public ChangeBoardOrderCommandValidator()
    {
        RuleFor(x => x.Order).ExclusiveBetween(1, int.MaxValue);
    }
}
