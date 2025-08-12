
using Application.Subscription.InviteUser;
using Domain.Subscriptions;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Subscriptions;

internal sealed class Invite : IEndpoint
{
    public sealed record Request(string Email, Guid WorkspaceId, Role Role);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("workspace/invite", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new InviteUserCommand(request.Email, request.WorkspaceId, request.Role);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithName("Invite Member")
        .WithSummary("Invite user to participant in workspace")
        .RequireAuthorization()
        .WithTags(Tags.Subscriptions);
    }
}
