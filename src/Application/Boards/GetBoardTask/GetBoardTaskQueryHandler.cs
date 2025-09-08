using System.Globalization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Common;
using Application.Users.Access;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.GetBoardTask;

internal sealed class GetBoardTaskQueryHandler(IApplicationDbContext context, IUserAccess userAccess) : IQueryHandler<GetBoardTaskQuery, PaginatedResponse<BoardResponse>>
{
    public async Task<Result<PaginatedResponse<BoardResponse>>> Handle(GetBoardTaskQuery request, CancellationToken cancellationToken)
    {
        Guid workspaceId = await context.Projects
             .AsNoTracking()
             .Include(x => x.Workspace)
             .Where(x => x.Id == request.ProjectId)
             .Select(x => x.Workspace.Id)
             .SingleOrDefaultAsync(cancellationToken);

        bool hasAccess = await userAccess.IsAuthenticatedAsync(workspaceId);
        if (hasAccess)
        {
            return Result.Failure<PaginatedResponse<BoardResponse>>(ProjectErrors.NotFound(request.ProjectId));
        }

        return await context.Boards
            .Include(x => x.Tasks)
            .AsNoTracking()
            .Where(x => x.Project.Id == request.ProjectId)
            .Select(x => new BoardResponse
            {
                Color = x.Color,
                Id = x.Id,
                IsArchive = x.IsArchive,
                Name = x.Name,
                Order = x.Order,
                TaskResponses = x.Tasks.Select(t => new TaskResponse
                {
                    Id = t.Id,
                    DeadLine = t.DeadLine.GetValueOrDefault().ToString(new CultureInfo("en-US")),
                    Description = t.Description,
                    Name = t.Name,
                    Order = t.Order,
                    Priority = t.Priority,
                    IsComplete = t.IsComplete
                }).ToList()
            })
            .ToPagedAsync(request, cancellationToken);
    }
}
