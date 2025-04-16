using Application.Abstractions.Messaging;

namespace Application.Boards.Get;
public sealed record GetBoardsQuery(int Page, int PageSize, Guid ProjectId) :IQuery<List<BoardResponse>>;
