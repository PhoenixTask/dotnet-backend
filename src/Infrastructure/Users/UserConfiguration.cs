using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Users;

internal sealed class UserConfiguration : CommonEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u=>u.UserName).IsRequired();

        builder.HasIndex(u => u.NormalizedUserName);

        builder.Property(u => u.ProfileImage).IsRequired(false);

        base.Configure(builder);
    }
}
