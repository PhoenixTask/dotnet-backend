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
            var command = new GetTasksQuery(boardId);

            Result<List<TaskResponse>> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Tasks);
    }
}
