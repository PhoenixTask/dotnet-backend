using Application.Boards.Get;
using Application.Boards.GetById;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Boards;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("board/{Id}", async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetBoardByIdQuery(Id);

            Result<BoardResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Get Board")
        .WithSummary("Get existing board")
        .RequireAuthorization()
        .WithTags(Tags.Boards);
    }
}
