using Application.Workspaces.Get;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Workspaces;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("workspace", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GetWorkspacesQuery();

            Result<List<WorkspaceResponse>> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Workspaces);
    }
}
