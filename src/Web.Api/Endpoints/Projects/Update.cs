using Application.Projects.Update;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Projects;

internal sealed class Update : IEndpoint
{
    public sealed record Request(string Name,string Color);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("project/update/{Id}", async (Guid Id , Request request ,ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateProjectCommand(
                Id,
                request.Name,
                request.Color);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Projects);
    }
}
