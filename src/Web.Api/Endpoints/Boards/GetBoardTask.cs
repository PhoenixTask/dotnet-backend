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
        app.MapGet("project/{projectId}/get-board-task", async (Guid projectId, ISender sender, CancellationToken cancellationToken, int page = 1, int pageSize = 20) =>
        {
            var command = new GetBoardTaskQuery(projectId, page, pageSize);

            Result<List<BoardResponse>> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Boards);
    }
}
