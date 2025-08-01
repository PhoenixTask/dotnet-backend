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
        .WithName("Update Board Order")
        .WithSummary("Update order of existing board")
       .RequireAuthorization()
       .AdvertisesApiVersion(2)
       .WithTags(Tags.Boards);

        app.MapPatch("board/update-order", async (ChangeBoardsOrderCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithName("Update Board Orders")
        .WithSummary("Update order of existing boards")
       .RequireAuthorization()
       .HasApiVersion(2)
       .WithTags(Tags.Boards);
    }
}
