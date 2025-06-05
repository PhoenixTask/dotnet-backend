using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Projects;

internal sealed class BoardConfiguration : CommonEntityConfiguration<Board>
{
    public override void Configure(EntityTypeBuilder<Board> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasMany(x => x.Tasks)
            .WithOne(x => x.Board)
            .HasForeignKey(x => x.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Project)
            .WithMany(p => p.Boards)
            .HasForeignKey(x=>x.ProjectId)
            .IsRequired();

        base.Configure(builder);
    }
}
