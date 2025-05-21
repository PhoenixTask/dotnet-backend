using System.Globalization;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.AccessAction;
using Domain.Projects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.GetBoardTask;

internal sealed class GetBoardTaskQueryHandler(IApplicationDbContext context,IUserContext userContext,ISender sender) : IQueryHandler<GetBoardTaskQuery, List<BoardResponse>>
{
    public async Task<Result<List<BoardResponse>>> Handle(GetBoardTaskQuery request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;

        UserAccessCommand accessRequest = new(userId, request.ProjectId, typeof(Project));
        Result hasAccess = await sender.Send(accessRequest, cancellationToken);
        if (hasAccess.IsFailure)
        {
            return Result.Failure<List<BoardResponse>>(hasAccess.Error);
        }

        return await context.Boards
            .Include(x => x.Tasks)
            .AsNoTracking()
            .Where(x => x.Project.Id == request.ProjectId)
            .Select(x => ToBoardResponse(x))
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
    }

    private static BoardResponse ToBoardResponse(Board x)
    {
        return new BoardResponse
        {
            Color = x.Color,
            Id = x.Id,
            IsArchive = x.IsArchive,
            Name = x.Name,
            Order = x.Order,
            TaskResponses = ToTaskResponseList(x)
        };
    }
    private static List<TaskResponse> ToTaskResponseList(Board x) 
    {
        return x.Tasks.Select(t => new TaskResponse
        {
            Id = t.Id,
            DeadLine = t.DeadLine.GetValueOrDefault().ToString(new CultureInfo("en-US")),
            Description = t.Description,
            Name = t.Name,
            Order = t.Order,
            Priority = t.Priority,
            IsComplete = t.IsComplete
        }).Take(5).ToList();
    }
}
