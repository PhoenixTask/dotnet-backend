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
        app.MapGet("workspace/{page}/{pageSize}", async (ISender sender, CancellationToken cancellationToken, int page = 1, int pageSize = 1000) =>
        {
            var command = new GetWorkspacesQuery(
                page,
                pageSize);

            Result<List<WorkspaceResponse>> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Workspaces);
    }
}
