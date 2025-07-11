using Application.Abstractions.Messaging;

namespace Application.Boards.ChangeOrder;
public sealed record ChangeBoardsOrderCommand(List<OrderableModel> Boards) : ICommand;
