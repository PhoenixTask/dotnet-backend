using Application.Abstractions.Messaging;

namespace Application.Tasks.Complete;
public sealed record CompleteTaskCommand(Guid TaskId, bool IsComplete) : ICommand;
