using Application.Abstractions.Messaging;

namespace Application.Comments.Delete;
public sealed record DeleteCommentCommand(Guid CommentId) : ICommand;
