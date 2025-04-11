using Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Settings;
internal sealed class SettingConfiguration : CommonEntityConfiguration<Setting>
{
    public override void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasIndex(s=>s.Key).IsUnique();

        base.Configure(builder);
    }
}
