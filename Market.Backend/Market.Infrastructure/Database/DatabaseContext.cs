using Market.Application.Abstractions;
using Market.Domain.Entities.Agencies;
using Market.Domain.Entities.Finance;


namespace Market.Infrastructure.Database;

public partial class DatabaseContext : DbContext, IAppDbContext
{
    

    public DbSet<MarketUserEntity> Users => Set<MarketUserEntity>();
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();
    public DbSet<PasswordResetTokenEntity> PasswordResetTokens => Set<PasswordResetTokenEntity>();

    public DbSet<AgencyEntity> Agencies => Set<AgencyEntity>();

    public DbSet<CurrencyEntity> Currencies => Set<CurrencyEntity>();
    public DbSet<ExchangeRateEntity> ExchangeRates => Set<ExchangeRateEntity>();

    private readonly TimeProvider _clock;
    public DatabaseContext(DbContextOptions<DatabaseContext> options, TimeProvider clock) : base(options)
    {
        _clock = clock;
    }
}
