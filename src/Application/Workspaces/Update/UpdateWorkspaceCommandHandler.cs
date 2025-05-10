using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.AccessAction;
using Domain.Subscriptions;
using Domain.Users;
using Domain.Workspaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Workspaces.Update;

internal sealed class UpdateWorkspaceCommandHandler(IApplicationDbContext context, IUserContext userContext,
    ISender sender)
    : ICommandHandler<UpdateWorkspaceCommand>
{
    public async Task<Result> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        Workspace? workspace = await context.Workspaces
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (workspace is null)
        {
            return Result.Failure(WorkspaceErrors.NotFound(request.Id));
        }

        UserAccessCommand accessRequest = new(userContext.UserId, workspace.Id, typeof(Workspace),ProjectRole.Manager);
        Result accessResult = await sender.Send(accessRequest, cancellationToken);
        if (accessResult.IsFailure)
        {
            return accessResult;
        }

        workspace.Name = request.Name.Trim();
        workspace.Color = request.Color.Trim();

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
