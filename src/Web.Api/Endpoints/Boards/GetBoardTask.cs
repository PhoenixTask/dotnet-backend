using Application.Boards.GetBoardTask;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Boards;

internal sealed class GetBoardTask : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("project/{projectId}/get-board-task", async (Guid projectId, int page, int pageSize, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GetBoardTaskQuery(projectId, page, pageSize);

            Result<List<BoardResponse>> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Boards);
    }
}
