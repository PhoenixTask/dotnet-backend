using Application.Tasks.Update;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class Update : IEndpoint
{
    public sealed record Request(string Name, Guid BoardId, string Description, DateTime? DeadLine, int Order, int Priority);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("task/update/{Id}", async (Guid Id , Request request ,ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateTaskCommand
            {
                BoardId = request.BoardId,
                DeadLine = request.DeadLine,
                Order = request.Order,  
                Priority = request.Priority,
                Description = request.Description,
                Id = Id,
                Name = request.Name
            };

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Tasks);
    }
}
