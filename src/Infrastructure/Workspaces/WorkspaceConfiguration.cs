using Domain.Workspaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Workspaces;
internal sealed class WorkspaceConfiguration : CommonEntityConfiguration<Workspace>
{
    public override void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.HasKey(x => x.Id);

        base.Configure(builder);
    }
}
