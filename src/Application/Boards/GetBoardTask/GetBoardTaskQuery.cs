using Application.Abstractions.Messaging;

namespace Application.Boards.GetBoardTask;
public sealed record GetBoardTaskQuery(Guid ProjectId,int Page, int PageSize) : IQuery<List<BoardResponse>>;
