using Market.Domain.Entities.Agencies;
using Market.Domain.Entities.Booking;
using Market.Domain.Entities.Finance;
using Market.Domain.Entities.Notifications;

namespace Market.Application.Abstractions;

// Application layer
public interface IAppDbContext
{
  
   

    DbSet<MarketUserEntity> Users { get; } 
    DbSet<RefreshTokenEntity> RefreshTokens { get; } 
    DbSet<PasswordResetTokenEntity> PasswordResetTokens { get; } 

    DbSet<AgencyEntity> Agencies { get; }
    DbSet<TourSlotEntity> TourSlots { get; }
    DbSet<ReservationEntity> Reservations { get; }
    DbSet<PaymentEntity> Payments { get; }
    DbSet<CurrencyEntity> Currencies { get; }
    DbSet<ExchangeRateEntity> ExchangeRates { get; }
    DbSet<NotificationEntity> Notifications { get; }
    Task<int> SaveChangesAsync(CancellationToken ct); 
} 
