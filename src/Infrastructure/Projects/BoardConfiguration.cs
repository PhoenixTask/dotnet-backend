using Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Projects;

internal sealed class BoardConfiguration : CommonEntityConfiguration<Board>
{
    public override void Configure(EntityTypeBuilder<Board> builder)
    {
        builder.HasKey(b => b.Id);

        base.Configure(builder);
    }
}
