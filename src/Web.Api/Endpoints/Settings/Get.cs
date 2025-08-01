using Application.Users.GetSetting;
using Application.Users.GetSettings;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Settings;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("user/setting/{Key}", async (string Key, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetSettingQuery(Key);

            Result<string> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Get Setting")
        .WithSummary("Get setting by key")
        .RequireAuthorization()
        .WithTags(Tags.Settings);

        app.MapGet("user/setting", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetSettingsQuery();

            Result<Dictionary<string, string>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Get Settings")
        .WithSummary("List User's Settings")
        .RequireAuthorization()
        .WithTags(Tags.Settings);
    }
}
