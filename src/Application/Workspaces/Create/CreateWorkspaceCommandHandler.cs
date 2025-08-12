using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Subscriptions;
using Domain.Workspaces;
using SharedKernel;

namespace Application.Workspaces.Create;

internal sealed class CreateWorkspaceCommandHandler(IApplicationDbContext context, IUserContext userContext)
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

        var owner = new TeamMember
        {
            UserId = userContext.UserId,
            Role = Role.Owner,
            Workspace = workspace
        };
        await context.Members.AddAsync(owner, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        return workspace.Id;
    }
}
