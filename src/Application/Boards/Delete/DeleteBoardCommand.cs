using Application.Abstractions.Messaging;

namespace Application.Boards.Delete;
public sealed record DeleteBoardCommand(Guid boardId) : ICommand;
