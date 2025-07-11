using Application.Abstractions.Messaging;
using Application.Boards.ChangeOrder;

namespace Application.Tasks.ChangeOrder;
public sealed record ChangeTasksOrderCommand(List<OrderableModel> Tasks) : ICommand;
