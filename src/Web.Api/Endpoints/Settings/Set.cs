using Application.Users.SetSetting;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Settings;

internal sealed class Set : IEndpoint
{
    public sealed record Request(string Key, string Value);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("user/setting/", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new SetSettingCommand(
                request.Key,
                request.Value);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Settings);
    }
}
