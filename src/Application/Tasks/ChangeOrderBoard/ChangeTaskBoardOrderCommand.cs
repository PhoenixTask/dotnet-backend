using Application.Abstractions.Messaging;

namespace Application.Tasks.ChangeOrderBoard;
public sealed record ChangeTaskBoardOrderCommand(List<TaskRequest> TaskRequests) : ICommand;
