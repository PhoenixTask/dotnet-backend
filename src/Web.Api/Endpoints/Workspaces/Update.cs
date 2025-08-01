using Application.Workspaces.Update;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Workspaces;

internal sealed class Update : IEndpoint
{
    public sealed record Request(string Name, string Description);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("workspace/{Id}", async (Guid Id, Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateWorkspaceCommand(Id, request.Name, request.Description);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithName("Update Workspace")
        .WithSummary("Update workspace information")
        .RequireAuthorization()
        .WithTags(Tags.Workspaces);
    }
}
