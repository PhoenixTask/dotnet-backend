using FluentValidation;

namespace Application.Boards.ChangeOrder;

internal sealed class ChangeBoardsOrderCommandValidator : AbstractValidator<ChangeBoardsOrderCommand>
{
    public ChangeBoardsOrderCommandValidator()
    {
        RuleFor(x => x.Boards)
             .NotEmpty();

        RuleForEach(x => x.Boards)
            .ChildRules(board =>
            {
                board.RuleFor(b => b.Order)
                     .GreaterThanOrEqualTo(1);

                board.RuleFor(b => b.Id)
                     .NotEmpty();
            });
    }
}
