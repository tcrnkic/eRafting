// TourSlotConfiguration.cs
using Rafting.Domain.Entities.Booking;
namespace Rafting.Infrastructure.Database.Configurations.Booking;

public sealed class TourSlotConfiguration : IEntityTypeConfiguration<TourSlotEntity>
{
    public void Configure(EntityTypeBuilder<TourSlotEntity> builder)
    {
        builder.ToTable("TourSlots");

        builder.Property(x => x.CancelReason)
            .HasMaxLength(TourSlotEntity.Constraints.CancelReasonMaxLength);

        // Kalendar: termini jedne ture po datumu.
        builder.HasIndex(x => new { x.TourId, x.StartsAtUtc });
    }
}