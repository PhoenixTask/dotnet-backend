using Domain.Projects;
using Domain.Subscriptions;
using Domain.Users;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using Task = Domain.Tasks.Task;


namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Workspace> Workspaces { get; }
    DbSet<Project> Projects { get; }
    DbSet<Board> Boards { get; }
    DbSet<Task> Tasks { get; }
    DbSet<Invitation> Invitations { get; }
    DbSet<Setting> Settings{ get; }
    DbSet<TeamMember> Members{ get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
