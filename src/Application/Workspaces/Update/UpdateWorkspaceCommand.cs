using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Workspaces.Update;
public sealed record UpdateWorkspaceCommand(Guid Id , string Name , string Color) : ICommand<Result>;
