using Application.Tasks.ChangeOrder;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class UpdateTaskOrder : IEndpoint
{
    public sealed record Request(Guid TaskId, int Order);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("task/update-order", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new ChangeTaskOrderCommand(
               request.TaskId,
               request.Order);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithName("Update Task Order")
        .WithSummary("Update task order within a board")
       .RequireAuthorization()
       .WithTags(Tags.Tasks);

        app.MapPatch("task/update-order", async (ChangeTasksOrderCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithName("Update Tasks Order")
        .WithSummary("Update list of tasks order")
        .RequireAuthorization()
        .HasApiVersion(2)
        .WithTags(Tags.Tasks);
    }
}
