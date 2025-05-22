using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.AccessAction;
using Application.Workspaces.Get;
using Domain.Workspaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Workspaces.GetById;

internal sealed class GetWorkspaceByIdQueryHandler(IUserContext userContext, ISender sender, IApplicationDbContext context) : IQueryHandler<GetWorkspaceByIdQuery, WorkspaceResponse>
{
    public async Task<Result<WorkspaceResponse>> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
    {
        bool workspaceExists = await context.Workspaces
            .AnyAsync(x => x.Id == request.WorkspaceId, cancellationToken);

        if (!workspaceExists)
        {
            return Result.Failure<WorkspaceResponse>(WorkspaceErrors.NotFound(request.WorkspaceId));
        }

        UserAccessCommand accessRequest = new(userContext.UserId, request.WorkspaceId, typeof(Workspace));
        Result accessResult = await sender.Send(accessRequest, cancellationToken);
        if (accessResult.IsFailure)
        {
            return Result.Failure<WorkspaceResponse>(accessResult.Error);
        }

        return await context.Workspaces
            .AsNoTracking()
            .Select(x => new WorkspaceResponse
            {
                Id = x.Id,
                Color = x.Color,
                Name = x.Name,
            })
            .SingleAsync(x => x.Id == request.WorkspaceId, cancellationToken);
    }
}
