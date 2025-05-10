using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Workspaces.Get;

internal sealed class GetWorkspacesQueryHandler(IApplicationDbContext context, IUserContext userContext)
    : IQueryHandler<GetWorkspacesQuery, List<WorkspaceResponse>>
{
    public async Task<Result<List<WorkspaceResponse>>> Handle(GetWorkspacesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Workspace> myWorkspaces = context.Workspaces
            .Where(x => x.CreatedById == userContext.UserId);

        IQueryable<Workspace> sharedWorkspaces = context.Members
           .Where(x => x.UserId == userContext.UserId)
           .Select(x=>x.Workspace);

        return await myWorkspaces.Concat(sharedWorkspaces).Select(x => new WorkspaceResponse
         {
             Id = x.Id,
             Name = x.Name,
             Color = x.Color,
         })
         .ToListAsync(cancellationToken);
    }
}
