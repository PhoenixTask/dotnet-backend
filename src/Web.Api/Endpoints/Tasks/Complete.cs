using Application.Tasks.Complete;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class Complete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("task/complete/{Id}", async (Guid Id, bool isComplete, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CompleteTaskCommand(Id, isComplete);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithName("Complete Task")
        .WithSummary("Mark a task as completed")
        .RequireAuthorization()
        .WithTags(Tags.Tasks);
    }
}
