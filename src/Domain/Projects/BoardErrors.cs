using SharedKernel;

namespace Domain.Projects;
public static class BoardErrors
{
    public static Error NotFound(Guid boardId) => Error.NotFound(
        "Board.NotFound",
        $"The board with the Id = '{boardId}' was not found");
}
