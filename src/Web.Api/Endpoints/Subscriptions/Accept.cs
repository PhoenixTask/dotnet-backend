
using Application.Subscription.AcceptInvite;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Subscriptions;

internal sealed class Accept : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("workspace/invite/accept/{token}", async (string token, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new AcceptInviteCommand(token);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
       .RequireAuthorization()
       .WithTags(Tags.Subscriptions);
    }
}
