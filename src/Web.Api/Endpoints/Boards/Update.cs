using Application.Boards.Update;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Boards;

internal sealed class Update : IEndpoint
{
    public sealed record Request(string Name, string Color, int Order);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("board/{Id}", async (Guid Id , Request request ,ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateBoardCommand(
               Id,
               request.Name,
               request.Color,
               request.Order);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Boards);
    }
}
