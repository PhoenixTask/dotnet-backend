using Application.Boards.GetBoardTask;
using Application.Common;
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
            var query = new GetBoardTaskQuery(projectId, page, pageSize);

            Result<PaginatedResponse<BoardResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("List Board Task")
        .WithSummary("Get existing board with their tasks")
        .RequireAuthorization()
        .WithTags(Tags.Boards);
    }
}
