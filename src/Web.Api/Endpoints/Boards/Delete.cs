using Application.Boards.Delete;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Boards;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("board/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DeleteBoardCommand(id);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithName("Delete Board")
        .WithSummary("Delete existing board")
        .RequireAuthorization()
        .WithTags(Tags.Boards);
    }
}
