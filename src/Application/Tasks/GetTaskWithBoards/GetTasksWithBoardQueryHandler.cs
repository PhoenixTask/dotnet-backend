using System.Globalization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Tasks.GetTaskByDate;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.GetTaskWithBoards;

internal sealed class GetTasksWithBoardQueryHandler(IApplicationDbContext context) : IQueryHandler<GetTasksWithBoardQuery, List<TaskResponse>>
{
    public async Task<Result<List<TaskResponse>>> Handle(GetTasksWithBoardQuery request, CancellationToken cancellationToken)
    {
        return await context.Tasks
               .Include(x => x.Board)
               .Where(x => x.Board.ProjectId == request.ProjectId)
               .Select(x => new TaskResponse
               {
                   BoardId = x.BoardId,
                   BoardName = x.Board.Name,
                   DeadLine = x.DeadLine.GetValueOrDefault().ToString(new CultureInfo("en-US")),
                   Name = x.Name,
                   Id = x.Id,
                   IsComplete = x.IsComplete,
                   Order = x.Order,
                   Priority = x.Priority
               }).ToListAsync(cancellationToken);
    }
}
