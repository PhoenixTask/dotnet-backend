using Application.Abstractions.Messaging;
using Application.Boards.Get;
using Application.Projects.Get;

namespace Application.Boards.GetById;
public sealed record GetBoardByIdQuery(Guid BoardId) : IQuery<BoardResponse>;
