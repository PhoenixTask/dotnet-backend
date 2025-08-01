using Application.Projects.Get;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Projects;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("workspace/{workspaceId}/project", async (Guid workspaceId, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetProjectsQuery(workspaceId);

            Result<List<ProjectResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("List Workspace's Projects")
        .WithSummary("Get list of project for a specified workspace")
        .RequireAuthorization()
        .WithTags(Tags.Projects);
    }
}
