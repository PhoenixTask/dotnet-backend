using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = Domain.Tasks.Task;

namespace Infrastructure.Tasks;
internal sealed class TaskConfiguration : CommonEntityConfiguration<Task>
{
    public override void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasOne(t => t.Board)
            .WithMany()
            .IsRequired();

        builder.Property(t => t.Attachment)
            .IsRequired(false);

        builder.Property(t => t.Thumbnail)
            .IsRequired(false);

        builder.Property(t=>t.DeadLine)
            .IsRequired(false);

        builder.HasOne(x => x.Board)
            .WithMany(x => x.Tasks)
            .HasForeignKey(t => t.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.IsComplete)
            .IsRequired()
            .HasDefaultValue(false);

        base.Configure(builder);
    }
}
