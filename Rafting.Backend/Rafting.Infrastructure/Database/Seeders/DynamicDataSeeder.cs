using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rafting.Domain.Entities.Agencies;
using Rafting.Domain.Entities.Finance;
using Rafting.Domain.Entities.Identity;

namespace Rafting.Infrastructure.Database.Seeders;

/// <summary>
/// Demo data for development and tests (never production). It runs at every start,
/// so every step is idempotent: it creates only what is missing (users by email, agencies by slug).
/// Reference data (currencies) comes from the migration, see <see cref="StaticDataSeeder"/>.
/// </summary>
public static class DynamicDataSeeder
{
    // One password per role, only for local development.
    private const string AdminPassword = "Admin123!";
    private const string ManagerPassword = "Manager123!";
    private const string EmployeePassword = "Employee123!";
    private const string CustomerPassword = "Customer123!";

    private const int BamCurrencyId = 1; // BAM, comes from the migration (StaticDataSeeder)

    private static readonly PasswordHasher<RaftingUserEntity> Hasher = new();

    private sealed record UserSeed(
        string Email, string Firstname, string Lastname, string Phone, string City, string Password,
        bool IsAdmin = false, bool IsManager = false, bool IsEmployee = false);

    public static async Task SeedAsync(DatabaseContext context)
    {
        // The schema is not created here: DatabaseInitializer runs the migrations first
        // (SQL Server) or EnsureCreated (in-memory database in tests).
        await SeedUsersAndAgenciesAsync(context);
        await SeedExchangeRatesAsync(context);
    }

    private static async Task SeedUsersAndAgenciesAsync(DatabaseContext context)
    {
        // Platform admin: no agency.
        await EnsureUserAsync(context, new UserSeed(
            "admin@rafting.local", "Admin", "Rafting", "+38761000001", "Bihać", AdminPassword, IsAdmin: true));

        await SeedAgencyAsync(context, "Una Rafting Demo", "una-rafting-demo",
            owner: new UserSeed(
                "manager@rafting.local", "Manager", "Rafting", "+38761000002", "Jezerski", ManagerPassword, IsManager: true),
            skippers:
            [
                new UserSeed("employee@rafting.local", "Employee", "Rafting", "+38761000003", "Bosanska Otoka", EmployeePassword, IsEmployee: true),
                new UserSeed("employee2@rafting.local", "Kenan", "Hodžić", "+38761000009", "Jezerski", EmployeePassword, IsEmployee: true)
            ]);

        await SeedAgencyAsync(context, "Una-Buk Rafting Demo", "una-buk-rafting-demo",
            owner: new UserSeed(
                "manager.unabuk@rafting.local", "Emir", "Zukić", "+38761000010", "Bosanska Otoka", ManagerPassword, IsManager: true),
            skippers:
            [
                new UserSeed("employee.unabuk@rafting.local", "Lejla", "Kovač", "+38761000011", "Bosanska Krupa", EmployeePassword, IsEmployee: true),
                new UserSeed("employee2.unabuk@rafting.local", "Haris", "Delić", "+38761000012", "Ljusina", EmployeePassword, IsEmployee: true)
            ]);

        // Guests: no agency, they register through the public page.
        await EnsureUserAsync(context, new UserSeed(
            "nina.bijedic@email.com", "Nina", "Bijedić", "+38761000006", "Sarajevo", CustomerPassword));
        await EnsureUserAsync(context, new UserSeed(
            "iris.memic@email.com", "Iris", "Memić", "+38761000007", "Mostar", CustomerPassword));
        await EnsureUserAsync(context, new UserSeed(
            "azra.smajic@email.com", "Azra", "Smajić", "+38761000008", "Tuzla", CustomerPassword));
    }

    /// <summary>
    /// Creates the agency (found by slug) with its owner and skippers if they do not exist yet.
    /// Order matters: the agency needs OwnerUserId, so the owner is saved first,
    /// and then AgencyId is set on the owner and the skippers.
    /// </summary>
    private static async Task SeedAgencyAsync(
        DatabaseContext context, string name, string slug, UserSeed owner, UserSeed[] skippers)
    {
        var ownerUser = await EnsureUserAsync(context, owner);

        var agency = await context.Agencies.FirstOrDefaultAsync(a => a.Slug == slug);
        if (agency is null)
        {
            agency = new AgencyEntity
            {
                Name = name,
                Slug = slug,
                Status = AgencyStatus.Active,
                OwnerUserId = ownerUser.Id,
                DefaultCurrencyId = BamCurrencyId
            };
            context.Agencies.Add(agency);
            await context.SaveChangesAsync(); // saved now so that agency.Id exists
        }

        ownerUser.AgencyId = agency.Id;
        foreach (var skipper in skippers)
        {
            var skipperUser = await EnsureUserAsync(context, skipper);
            skipperUser.AgencyId = agency.Id;
        }

        // If nothing changed (second run), EF sends no UPDATE.
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Returns the user with this email; creates it if it does not exist.
    /// Saves right away because the callers need the generated Id.
    /// </summary>
    private static async Task<RaftingUserEntity> EnsureUserAsync(DatabaseContext context, UserSeed seed)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == seed.Email);
        if (user is not null)
            return user;

        user = new RaftingUserEntity
        {
            Firstname = seed.Firstname,
            Lastname = seed.Lastname,
            Email = seed.Email,
            Phone = seed.Phone,
            City = seed.City,
            PasswordHash = Hasher.HashPassword(null!, seed.Password),
            IsAdmin = seed.IsAdmin,
            IsManager = seed.IsManager,
            IsEmployee = seed.IsEmployee,
            IsEnabled = true
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    /// <summary>
    /// Approximate rates for USD, CHF, GBP and RSD on four dates in 2026, so that conversion works right after the start.
    /// Currencies and the fixed EUR rate (1.95583) come from the migration (StaticDataSeeder).
    /// These are not real rates: in production the administrator enters them or they are fetched through ExchangeRates:Provider.
    /// </summary>
    private static async Task SeedExchangeRatesAsync(DatabaseContext context)
    {
        const string source = "Seed (približno)";

        if (await context.ExchangeRates.AnyAsync(r => r.Source == source))
            return;

        // KM per 1 unit of the currency, for 01.01, 01.04, 01.07 and 01.10.2026.
        var approximateRates = new Dictionary<string, decimal[]>
        {
            ["USD"] = [1.6650m, 1.6800m, 1.6600m, 1.6750m],
            ["CHF"] = [2.0900m, 2.1000m, 2.0850m, 2.0950m],
            ["GBP"] = [2.2400m, 2.2550m, 2.2300m, 2.2450m],
            ["RSD"] = [0.016690m, 0.016685m, 0.016692m, 0.016688m]
        };

        DateTime[] validFromDates =
        [
            new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc)
        ];

        var codes = approximateRates.Keys.ToList();
        var currencyIdsByCode = await context.Currencies
            .Where(c => codes.Contains(c.Code))
            .ToDictionaryAsync(c => c.Code, c => c.Id);

        var rates = new List<ExchangeRateEntity>();
        foreach (var (code, values) in approximateRates)
        {
            if (!currencyIdsByCode.TryGetValue(code, out var currencyId))
                continue;

            for (var i = 0; i < validFromDates.Length; i++)
            {
                rates.Add(new ExchangeRateEntity
                {
                    CurrencyId = currencyId,
                    RateToBase = values[i],
                    ValidFromUtc = validFromDates[i],
                    Source = source,
                    CreatedAtUtc = DateTime.UtcNow
                });
            }
        }

        context.ExchangeRates.AddRange(rates);
        await context.SaveChangesAsync();

        Console.WriteLine($"Dynamic seed: {rates.Count} approximate exchange rates added.");
    }
}