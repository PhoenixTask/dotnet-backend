using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Projects.Update;

public sealed record UpdateProjectCommand(Guid ProjectId,string Name,string Color) : ICommand<Result>;
