using Market.Domain.Entities.Notifications;

namespace Market.Infrastructure.Database.Configurations.Notifications;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<NotificationEntity>
{
    public void Configure(EntityTypeBuilder<NotificationEntity> builder)
    {
        builder.ToTable("Notifications");

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(NotificationEntity.Constraints.TitleMaxLength);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(NotificationEntity.Constraints.MessageMaxLength);

        builder.Property(x => x.RelatedEntityType)
            .HasMaxLength(NotificationEntity.Constraints.RelatedEntityTypeMaxLength);

        // Najčešći upit: nepročitane obavijesti jednog korisnika.
        builder.HasIndex(x => new { x.UserId, x.IsRead });

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}