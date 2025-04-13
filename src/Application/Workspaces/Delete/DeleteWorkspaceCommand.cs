using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Workspaces.Delete;
public sealed record DeleteWorkspaceCommand(Guid Id) : ICommand<Result>;
