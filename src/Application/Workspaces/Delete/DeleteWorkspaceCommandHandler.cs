using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Workspaces.Delete;

internal sealed class DeleteWorkspaceCommandHandler(IApplicationDbContext context,IUserContext userContext)
    : ICommandHandler<DeleteWorkspaceCommand, Result>
{
    public async Task<Result<Result>> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        Workspace? workspace =await context.Workspaces
            .SingleOrDefaultAsync(x =>x.Id == request.Id && x.CreatedById == userContext.UserId, cancellationToken: cancellationToken);
        
        if (workspace is null)
        {
            return Result.Failure(WorkspaceErrors.NotFound(request.Id));
        }

        context.Workspaces.Remove(workspace);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
