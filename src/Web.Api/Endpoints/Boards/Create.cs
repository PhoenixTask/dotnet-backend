using Application.Boards.Create;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Boards;

internal sealed class Create : IEndpoint
{
    public sealed record Request(Guid ProjectId, string Name, string Color, int Order);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("board/", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateBoardCommand(
                request.ProjectId,
                request.Name,
                request.Color);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Create Board")
        .WithSummary("Create new board")
        .RequireAuthorization()
        .WithTags(Tags.Boards);
    }
}
