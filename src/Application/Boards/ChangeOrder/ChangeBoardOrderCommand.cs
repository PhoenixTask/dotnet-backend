using Application.Abstractions.Messaging;

namespace Application.Boards.ChangeOrder;
public sealed record ChangeBoardOrderCommand(Guid BoardId, int Order) : ICommand;
