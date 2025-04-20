using Domain.Subscriptions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Subscriptions;
internal sealed class TeamMemberConfiguration : CommonEntityConfiguration<TeamMember>
{
    public override void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.HasKey(x => new { x.UserId, x.WorkspaceId });

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);

        builder.HasOne(x => x.Workspace)
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}
