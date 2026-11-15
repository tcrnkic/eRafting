using Rafting.Domain.Entities.Agencies;
using Rafting.Domain.Entities.Booking;
using Rafting.Domain.Entities.Finance;
using Rafting.Domain.Entities.Notifications;

namespace Rafting.Application.Abstractions;

// Application layer
public interface IAppDbContext
{
  
   

    DbSet<RaftingUserEntity> Users { get; } 
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
