using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Workspaces.Get;

internal sealed class GetWorkspacesQueryHandler(IApplicationDbContext context, IUserContext userContext)
    : IQueryHandler<GetWorkspacesQuery, List<WorkspaceResponse>>
{
    public async Task<Result<List<WorkspaceResponse>>> Handle(GetWorkspacesQuery request, CancellationToken cancellationToken)
    {
        return await context.Workspaces
            .Where(x => x.CreatedById == userContext.UserId )
            .Select(x => new WorkspaceResponse
            {
                Id = x.Id,
                Name = x.Name,
                Color = x.Color,
            })
            .Skip((request.Page -1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
    }
}
