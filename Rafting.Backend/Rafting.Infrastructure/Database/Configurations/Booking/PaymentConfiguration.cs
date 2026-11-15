// PaymentConfiguration.cs
using Rafting.Domain.Entities.Booking;
using Rafting.Domain.Entities.Finance;
namespace Rafting.Infrastructure.Database.Configurations.Booking;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        builder.ToTable("Payments");

        builder.Property(x => x.RateToBase)
            .HasPrecision(ExchangeRateEntity.Constraints.RatePrecision, ExchangeRateEntity.Constraints.RateScale);

        builder.HasOne(x => x.Reservation).WithMany(x => x.Payments)
            .HasForeignKey(x => x.ReservationId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Currency).WithMany()
            .HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}