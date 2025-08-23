using Application.Abstractions.Messaging;


namespace Application.Tasks.ChangeOrder;
public sealed record ChangeTaskOrderCommand(Guid TaskId, int Order) : ICommand;
