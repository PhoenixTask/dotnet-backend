using Application.Tasks.SwitchBoard;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class SwitchTaskBoard : IEndpoint
{
    public sealed record Request(Guid BoardId, Guid TaskId);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("task/update-board", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new SwitchBoardCommand(
               request.TaskId,
               request.BoardId);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
       .RequireAuthorization()
       .WithTags(Tags.Tasks);
    }
}
