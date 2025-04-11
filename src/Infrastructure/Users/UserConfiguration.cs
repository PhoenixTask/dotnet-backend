using Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Users;

internal sealed class UserConfiguration : CommonEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasIndex(u => u.UserName).IsUnique();

        base.Configure(builder);
    }
}
