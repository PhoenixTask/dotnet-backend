using Application.Tasks.UpdateDeadLine;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class UpdateDeadLine : IEndpoint
{
    public sealed record Request(Guid TaskId, DateTime? NewDeadLine);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("task/update-deadline", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateDeadLineCommand(request.TaskId, request.NewDeadLine);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Tasks);
    }
}
