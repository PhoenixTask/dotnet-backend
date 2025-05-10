
using Application.Users.Update;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class Update : IEndpoint
{
    public sealed record Request(string? FirstName ,string? LastName);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("user/update", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateUserCommand(request.FirstName, request.LastName);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Users);
    }
}
