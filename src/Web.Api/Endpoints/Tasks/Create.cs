using Application.Tasks.Create;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class Create : IEndpoint
{
    public sealed record Request(string Name, Guid BoardId, string Description, DateTime? DeadLine, int Priority);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("task/", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateTaskCommand
            {
                BoardId = request.BoardId,
                Priority = request.Priority,
                Name = request.Name,
                Description = request.Description,
                DeadLine = request.DeadLine
            };

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Create Task")
        .WithSummary("Create Task for a board")
        .RequireAuthorization()
        .WithTags(Tags.Tasks);
    }
}
