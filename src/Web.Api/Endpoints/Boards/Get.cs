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
        app.MapGet("project/{projectId}/board/{page}/{pageSize}", async (Guid projectId, ISender sender, CancellationToken cancellationToken, int page = 1, int pageSize = 1000) =>
        {
            var command = new GetBoardsQuery(
                page,
                pageSize,
                projectId);

            Result<List<BoardResponse>> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Boards);
    }
}
