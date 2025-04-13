using SharedKernel;

namespace Domain.Workspaces;
public static class WorkspaceErrors
{
    public static Error NotFound(Guid workspaceId) => Error.NotFound(
        "Workspace.NotFound",
        $"The workspace with the Id = '{workspaceId}' was not found");
}
