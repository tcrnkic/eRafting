// ReservationConfiguration.cs
using Market.Domain.Entities.Booking;
using Market.Domain.Entities.Finance;
namespace Market.Infrastructure.Database.Configurations.Booking;

public sealed class ReservationConfiguration : IEntityTypeConfiguration<ReservationEntity>
{
    public void Configure(EntityTypeBuilder<ReservationEntity> builder)
    {
        builder.ToTable("Reservations");

        builder.Property(x => x.ReferenceNumber)
            .IsRequired()
            .HasMaxLength(ReservationEntity.Constraints.ReferenceNumberMaxLength);

        builder.HasIndex(x => x.ReferenceNumber)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        // Kurs ima 6 decimala (kao u ExchangeRate), ostali iznosi 2.
        builder.Property(x => x.RateToBase)
            .HasPrecision(ExchangeRateEntity.Constraints.RatePrecision, ExchangeRateEntity.Constraints.RateScale);

        // Brojanje zauzetih mjesta po terminu i statusu.
        builder.HasIndex(x => new { x.TourSlotId, x.Status });

        builder.HasOne(x => x.GuestUser).WithMany()
            .HasForeignKey(x => x.GuestUserId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TourSlot).WithMany(x => x.Reservations)
            .HasForeignKey(x => x.TourSlotId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Currency).WithMany()
            .HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}