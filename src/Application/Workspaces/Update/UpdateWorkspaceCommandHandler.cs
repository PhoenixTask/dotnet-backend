using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Workspaces.Update;

internal sealed class UpdateWorkspaceCommandHandler(IApplicationDbContext context, IUserContext userContext)
    : ICommandHandler<UpdateWorkspaceCommand, Result>
{
    public async Task<Result<Result>> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        Domain.Workspaces.Workspace? workspace = await context.Workspaces
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.CreatedBy == new Domain.Users.User { Id = userContext.UserId }, cancellationToken);
        
        if(workspace is null)
        {
            return Result.Failure(WorkspaceErrors.NotFound(request.Id));
        }

        workspace.Name = request.Name.Trim();
        workspace.Color = request.Color.Trim();

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
