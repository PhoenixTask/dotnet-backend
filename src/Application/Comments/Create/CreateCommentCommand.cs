using Application.Abstractions.Messaging;

namespace Application.Comments.Create;
public sealed record CreateCommentCommand(Guid TaskId, string Content) : ICommand<Guid>;
