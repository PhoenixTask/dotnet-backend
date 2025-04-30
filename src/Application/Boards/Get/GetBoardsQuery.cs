using Application.Abstractions.Messaging;

namespace Application.Boards.Get;
public sealed record GetBoardsQuery(Guid ProjectId) :IQuery<List<BoardResponse>>;
