using Application.Workspaces.Create;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Workspaces;

internal sealed class Create : IEndpoint
{
    public sealed record Request(string Name, string Description);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("workspace/", async (Request request ,ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateWorkspaceCommand(request.Name, request.Description);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Create Workspace")
        .WithSummary("Create a new workspace")
        .RequireAuthorization()
        .WithTags(Tags.Workspaces);
    }
}
