using System.Globalization;
using System.Linq.Expressions;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Boards.GetBoardTask;
using Application.Users.AccessAction;
using Domain.Projects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.Search;

internal sealed class SearchTaskByProjectQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext,
    ISender sender
) : IQueryHandler<SearchTaskByProjectQuery, List<BoardResponse>>
{
    public async Task<Result<List<BoardResponse>>> Handle(SearchTaskByProjectQuery request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;

        var accessRequest = new UserAccessCommand(userId, request.ProjectId, typeof(Project));
        Result hasAccess = await sender.Send(accessRequest, cancellationToken);
        if (hasAccess.IsFailure)
        {
            return Result.Failure<List<BoardResponse>>(hasAccess.Error);
        }

        string parameter = request.Parameter?.Trim() ?? string.Empty;
        string likePattern = $"%{parameter}%";

        List<BoardResponse> boards = await context.Boards
            .AsNoTracking()
            .Where(b => b.Project.Id == request.ProjectId)
            .Select(b => new BoardResponse
            {
                Id = b.Id,
                Name = b.Name,
                Color = b.Color,
                IsArchive = b.IsArchive,
                Order = b.Order,
                TaskResponses = b.Tasks
                    .Where(t =>
                        EF.Functions.Like(t.Name, likePattern) ||
                        EF.Functions.Like(t.Description, likePattern))
                    .Select(t => new TaskResponse
                    {
                        Id = t.Id,
                        DeadLine = t.DeadLine.GetValueOrDefault().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Description = t.Description,
                        Name = t.Name,
                        Order = t.Order,
                        Priority = t.Priority
                    })
                    .ToList()
            })
            .Where(b => b.TaskResponses.Any())
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result.Success(boards);
    }
}
