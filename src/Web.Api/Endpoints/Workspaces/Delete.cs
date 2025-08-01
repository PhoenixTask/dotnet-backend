using Application.Workspaces.Delete;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Workspaces;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("workspace/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DeleteWorkspaceCommand(id);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithName("Delete Workspace")
        .WithSummary("Delete workspace by ID")
        .RequireAuthorization()
        .WithTags(Tags.Workspaces);
    }
}
