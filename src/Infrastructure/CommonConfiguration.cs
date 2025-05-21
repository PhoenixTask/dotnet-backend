using Domain.Interfaces;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure;
internal abstract class CommonEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        if (typeof(IAuditableEntity).IsAssignableFrom(typeof(TEntity)))
        {
            ConfigureAuditable(builder);
        }

        if (typeof(IBlamableEntity).IsAssignableFrom(typeof(TEntity)))
        {
            ConfigureBlamable(builder);
        }

        if (typeof(ISoftDeletableEntity).IsAssignableFrom(typeof(TEntity)))
        {
            ConfigureSoftDelete(builder);
        }
    }
    private static void ConfigureAuditable<T>(EntityTypeBuilder<T> builder)
        where T : class
    {
        builder.Property(x => ((IAuditableEntity)x).CreatedOnUtc).IsRequired(false);

        builder.Property(x => ((IAuditableEntity)x).ModifiedOnUtc).IsRequired(false);
    }
    private static void ConfigureBlamable<T>(EntityTypeBuilder<T> builder)
        where T : class
    {
        builder.HasOne(x => ((IBlamableEntity)x).CreatedBy)
            .WithMany()
            .HasForeignKey(x => ((IBlamableEntity)x).CreatedById)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasOne(x => ((IBlamableEntity)x).ModifiedBy)
            .WithMany()
            .HasForeignKey(x => ((IBlamableEntity)x).ModifiedById)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    }
    private static void ConfigureSoftDelete<T>(EntityTypeBuilder<T> builder)
        where T : class
    {
        builder.Property(x => ((ISoftDeletableEntity)x).DeletedOnUtc).IsRequired(false);

        builder.HasOne(x => ((ISoftDeletableEntity)x).DeletedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.Property<bool>(x => ((ISoftDeletableEntity)x).Deleted).HasDefaultValue(false);

        builder.HasQueryFilter(x => !((ISoftDeletableEntity)x).Deleted);
    }
}
