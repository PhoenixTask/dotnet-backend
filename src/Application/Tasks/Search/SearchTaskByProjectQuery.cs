using Application.Abstractions.Messaging;
using Application.Boards.GetBoardTask;

namespace Application.Tasks.Search;
public sealed record SearchTaskByProjectQuery(Guid ProjectId, string Parameter, int Page, int PageSize) : IQuery<List<BoardResponse>>;
