using Application.Abstractions.Messaging;

namespace Application.Comments.Get;
public sealed record GetTaskCommentsQuery(Guid TaskId) : IQuery<List<CommentResponse>>;
