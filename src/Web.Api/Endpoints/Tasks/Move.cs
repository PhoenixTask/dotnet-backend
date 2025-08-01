using Application.Tasks.ChangeOrderBoard;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

public sealed class Move : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("task/move", async (ChangeTaskBoardOrderCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithName("Move Task")
        .WithSummary("Move task to a different board with new order")
       .RequireAuthorization()
       .WithTags(Tags.Tasks);
    }
}
