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

        base.Configure(builder);
    }
}
