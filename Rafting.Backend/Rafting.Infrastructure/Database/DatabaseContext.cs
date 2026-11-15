using Rafting.Application.Abstractions;
using Rafting.Domain.Entities.Agencies;
using Rafting.Domain.Entities.Booking;
using Rafting.Domain.Entities.Finance;
using Rafting.Domain.Entities.Notifications;


namespace Rafting.Infrastructure.Database;

public partial class DatabaseContext : DbContext, IAppDbContext
{
    

    public DbSet<RaftingUserEntity> Users => Set<RaftingUserEntity>();
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();
    public DbSet<PasswordResetTokenEntity> PasswordResetTokens => Set<PasswordResetTokenEntity>();

    public DbSet<AgencyEntity> Agencies => Set<AgencyEntity>();
    public DbSet<TourSlotEntity> TourSlots => Set<TourSlotEntity>();
    public DbSet<ReservationEntity> Reservations => Set<ReservationEntity>();
    public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();
    public DbSet<CurrencyEntity> Currencies => Set<CurrencyEntity>();
    public DbSet<ExchangeRateEntity> ExchangeRates => Set<ExchangeRateEntity>();
    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();
    private readonly TimeProvider _clock;
    public DatabaseContext(DbContextOptions<DatabaseContext> options, TimeProvider clock) : base(options)
    {
        _clock = clock;
    }
}
