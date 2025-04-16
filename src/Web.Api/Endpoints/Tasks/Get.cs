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
        app.MapGet("board/{boardId}/task/{page}/{pageSize}", async (Guid boardId, ISender sender, CancellationToken cancellationToken, int page = 1, int pageSize = 1000) =>
        {
            var command = new GetTasksQuery(
                page,
                pageSize,
                boardId);

            Result<List<TaskResponse>> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Tasks);
    }
}
