using System.Globalization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Tasks.Get;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.GetById;

internal sealed class GetTaskByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetTaskByIdQuery, TaskResponse>
{
    public async Task<Result<TaskResponse>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        bool taskExists = await context.Tasks
          .AnyAsync(x => x.Id == request.TaskId, cancellationToken);

        if (!taskExists)
        {
            return Result.Failure<TaskResponse>(TaskErrors.NotFound(request.TaskId));
        }

        return await context.Tasks
            .AsNoTracking()
            .Select(x => new TaskResponse
            {
                Id = x.Id,
                DeadLine = x.DeadLine.GetValueOrDefault().ToString(new CultureInfo("en-US")),
                Description = x.Description,
                Name = x.Name,
                Order = x.Order,
                Priority = x.Priority,
                IsComplete = x.IsComplete
            })
            .SingleAsync(x => x.Id == request.TaskId, cancellationToken);
    }
}
