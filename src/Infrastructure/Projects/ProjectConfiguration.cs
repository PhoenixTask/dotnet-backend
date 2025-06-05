using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project = Domain.Projects.Project;

namespace Infrastructure.Projects;
internal sealed class ProjectConfiguration : CommonEntityConfiguration<Project>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Workspace)
            .WithMany(w => w.Projects)
            .IsRequired();

        base.Configure(builder);
    }
}
