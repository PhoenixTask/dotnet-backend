using Application.Projects.Create;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Projects;

internal sealed class Create : IEndpoint
{
    public sealed record Request(string Name,Guid WorkspaceId);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("project/create/", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateProjectCommand(
                request.Name,
                request.WorkspaceId);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Projects);
    }
}
