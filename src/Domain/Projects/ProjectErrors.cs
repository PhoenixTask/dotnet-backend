using SharedKernel;

namespace Domain.Projects;

public static class ProjectErrors
{
    public static Error NotFound(Guid projectId) => Error.NotFound(
        "Project.NotFound",
        $"The project with the Id = '{projectId}' was not found");
}
