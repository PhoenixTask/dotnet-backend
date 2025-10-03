using Application.Tasks.GetTaskByDate;
using Application.Tasks.GetTaskWithBoards;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class GetTasksWithBoard : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("task/tasks-with-board", async (Guid ProjectId, ISender sender, CancellationToken cancellationToken, bool IncludeCompled = true) =>
        {
            var query = new GetTasksWithBoardQuery(ProjectId, IncludeCompled);

            Result<List<TaskResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Get Tasks With Board")
        .WithSummary("Get tasks it's boards name and id for a project")
        .RequireAuthorization()
        .WithTags(Tags.Tasks);
    }
}
