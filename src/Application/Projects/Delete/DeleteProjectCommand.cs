using Application.Abstractions.Messaging;

namespace Application.Projects.Delete;

public sealed record DeleteProjectCommand(Guid ProjectId) : ICommand;
