using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Workspaces;
using SharedKernel;

namespace Application.Workspaces.Create;

internal sealed class CreateWorkspaceCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CreateWorkspaceCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var workspace = new Workspace
        {
            Name = request.Name.Trim(),
            Color = request.Color.Trim(),
        };
        await context.Workspaces.AddAsync(workspace, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return workspace.Id;
    }
}
