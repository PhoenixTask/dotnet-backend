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
        app.MapGet("workspace/{workspaceId}/project/{page}/{pageSize}", async (Guid workspaceId,ISender sender, CancellationToken cancellationToken, int page = 1, int pageSize = 1000) =>
        {
            var command = new GetProjectsQuery(
                page,
                pageSize,
                workspaceId);

            Result<List<ProjectResponse>> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Projects);
    }
}
