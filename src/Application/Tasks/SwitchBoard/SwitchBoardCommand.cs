using Application.Abstractions.Messaging;


namespace Application.Tasks.SwitchBoard;
public sealed record SwitchBoardCommand(Guid TaskId,Guid BoardId) : ICommand;
