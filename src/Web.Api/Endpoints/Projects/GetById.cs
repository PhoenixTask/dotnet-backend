using Application.Projects.Get;
using Application.Projects.GetById;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Projects;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("project/{Id}", async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetProjectByIdQuery(Id);

            Result<ProjectResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Get Project")
        .WithSummary("Get existing project")
        .RequireAuthorization()
        .WithTags(Tags.Projects);
    }
}
