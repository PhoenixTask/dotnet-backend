using Application.Boards.GetBoardTask;
using Application.Tasks.Search;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class Search : IEndpoint
{
    //public sealed record Reqeust(Guid ProjectId, string Term, int Page, int PageSize);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("project/{projectId}/task/search", async (Guid projectId, string term, ISender sender, CancellationToken cancellationToken, int page = 1, int pageSize = 20) =>
        {
            var query = new SearchTaskByProjectQuery(projectId, term, page, pageSize);

            Result<List<BoardResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Tasks);
    }
}
