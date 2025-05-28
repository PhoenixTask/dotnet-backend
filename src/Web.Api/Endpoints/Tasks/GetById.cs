using Application.Tasks.Get;
using Application.Tasks.GetById;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("task/{Id}", async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetTaskByIdQuery(Id);

            Result<TaskResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Tasks);
    }
}
