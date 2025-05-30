using Application.Boards.ChangeOrder;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Boards;

internal sealed class UpdateBoardOrder : IEndpoint
{
    public sealed record Request(Guid BoardId, int Order);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("board/update-order", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new ChangeBoardOrderCommand(
               request.BoardId,
               request.Order);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
       .RequireAuthorization()
       .WithTags(Tags.Boards);
    }
}
