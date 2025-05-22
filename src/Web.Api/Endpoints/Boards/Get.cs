using Application.Boards.Get;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Boards;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("project/{projectId}/board", async (Guid projectId, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetBoardsQuery(projectId);

            Result<List<BoardResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Boards);
    }
}
