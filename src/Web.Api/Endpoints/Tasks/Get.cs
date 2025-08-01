using Application.Tasks.Get;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("board/{boardId}/task", async (Guid boardId, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetTasksQuery(boardId);

            Result<List<TaskResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("List Board's Tasks")
        .WithSummary("Get list of task for a specified board")
        .RequireAuthorization()
        .WithTags(Tags.Tasks);
    }
}
