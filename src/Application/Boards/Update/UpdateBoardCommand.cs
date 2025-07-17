using Application.Abstractions.Messaging;

namespace Application.Boards.Update;
public sealed record UpdateBoardCommand(Guid BoardId, string Name, string Color) : ICommand;
