using SharedKernel;

namespace Domain.Tasks;
public static class CommentErrors
{
    public static Error NotFound(Guid commentId)
    {
        return Error.NotFound(
        "Comment.NotFound",
        $"The comment with the Id = '{commentId}' was not found");
    }
}
