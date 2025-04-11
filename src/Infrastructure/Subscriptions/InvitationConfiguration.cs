using Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Subscriptions;
internal sealed class InvitationConfiguration : CommonEntityConfiguration<Invitation>
{
    public override void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.HasKey(i => i.Id);

        builder.HasIndex(i=>i.Token)
            .IsUnique();

        builder.HasOne(x => x.Workspace)
            .WithMany()
            .IsRequired();

        builder.HasOne(i=>i.Invited)
            .WithMany()
            .IsRequired(false);

        builder.Property(x => x.IsApproved)
            .HasDefaultValue(false);

        base.Configure(builder);
    }
}
