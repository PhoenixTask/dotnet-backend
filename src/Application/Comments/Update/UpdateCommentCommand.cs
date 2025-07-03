using Application.Abstractions.Messaging;

namespace Application.Comments.Update;
public sealed record UpdateCommentCommand(Guid CommentId, string Content) : ICommand;
