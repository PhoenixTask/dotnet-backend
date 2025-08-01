using System.Globalization;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.GetTaskByDate;

internal sealed class GetTaskByDeadLineQueryHandler(IApplicationDbContext context, IUserContext userContext) : IQueryHandler<GetTaskByDeadLineQuery, List<TaskResponse>>
{
    public async Task<Result<List<TaskResponse>>> Handle(GetTaskByDeadLineQuery request, CancellationToken cancellationToken)
    {
        return await context.Tasks
            .AsNoTracking()
            .Include(x => x.Board)
            .Where(x => x.Board.ProjectId == request.ProjectId)
            .Where(x => x.DeadLine >= DateOnly.FromDateTime(request.StartDate))
            .Where(x => x.DeadLine <= DateOnly.FromDateTime(request.EndDate))
            .Where(x => x.CreatedById == userContext.UserId)
            .Select(x => new TaskResponse
            {
                DeadLine = x.DeadLine.GetValueOrDefault().ToString(new CultureInfo("en-US")),
                Id = x.Id,
                IsComplete = x.IsComplete,
                Name = x.Name,
                Order = x.Order,
                Priority = x.Priority,
                BoardId = x.Board.Id,
                BoardName = x.Board.Name,
            })
            .ToListAsync(cancellationToken);
    }
}
