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
        app.MapGet("workspace/", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetWorkspacesQuery();

            Result<List<WorkspaceResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Get Workspaces")
        .WithSummary("Get list of user's workspaces")
        .RequireAuthorization()
        .WithTags(Tags.Workspaces);
    }
}
