using Application.Workspaces.Get;
using Application.Workspaces.GetById;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Workspaces;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("workspace/{Id}", async (Guid Id,ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetWorkspaceByIdQuery(Id);

            Result<WorkspaceResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Workspaces);
    }
}
