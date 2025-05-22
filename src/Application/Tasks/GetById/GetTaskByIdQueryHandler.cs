using System.Globalization;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Tasks.Get;
using Application.Users.AccessAction;
using Domain.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.GetById;

internal sealed class GetTaskByIdQueryHandler(IUserContext userContext, ISender sender, IApplicationDbContext context) : IQueryHandler<GetTaskByIdQuery, TaskResponse>
{
    public async Task<Result<TaskResponse>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        bool taskExists = await context.Tasks
          .AnyAsync(x => x.Id == request.TaskId, cancellationToken);

        if (!taskExists)
        {
            return Result.Failure<TaskResponse>(TaskErrors.NotFound(request.TaskId));
        }

        UserAccessCommand accessRequest = new(userContext.UserId, request.TaskId, typeof(Domain.Tasks.Task));
        Result accessResult = await sender.Send(accessRequest, cancellationToken);
        if (accessResult.IsFailure)
        {
            return Result.Failure<TaskResponse>(accessResult.Error);
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
