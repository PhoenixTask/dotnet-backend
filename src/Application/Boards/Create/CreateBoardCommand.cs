using Application.Abstractions.Messaging;

namespace Application.Boards.Create;
public sealed record CreateBoardCommand(Guid ProjectId, string Name, string Color) : ICommand<Guid>;
