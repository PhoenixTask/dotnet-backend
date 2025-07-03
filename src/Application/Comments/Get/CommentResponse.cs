namespace Application.Comments.Get;

public sealed class CommentResponse
{
    public Guid CommentId { get; set; }
    public string Content { get; set; }
    public string UserName { get; set; }
    public string? FullName { get; set; }
    public DateTime? PublishedOn { get; set; }
}
