using Application.Abstractions.Messaging;

namespace Application.Tasks.UpdateDeadLine;
public sealed record UpdateDeadLineCommand(Guid TaskId, DateTime? NewDeadLine) : ICommand;
