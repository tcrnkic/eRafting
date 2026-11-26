using Microsoft.Extensions.Time.Testing;
using Rafting.Domain.Entities.Identity;
using Rafting.Infrastructure.Database.Seeders;

namespace Rafting.Tests.SeederTests.UnitTests
{
    public class DynamicDataSeederUnitTests
    {
        private readonly FakeTimeProvider _clock = new(new DateTimeOffset(2026, 10, 19, 12, 0, 0, TimeSpan.Zero));

        private DatabaseContext GetInMemoryDbContext()
        {
            // each test gets a new database
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new DatabaseContext(options, _clock);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task SeedAsync_CreatesTwoAgencies_EachWithOwnerAndTwoSkippers()
        {
            using var context = GetInMemoryDbContext();
            await DynamicDataSeeder.SeedAsync(context);

            var agencies = await context.Agencies.ToListAsync();
            Assert.Equal(2, agencies.Count);

            var users = await context.Users.ToListAsync();
            foreach (var agency in agencies)
            {
                var owner = users.Single(u => u.Id == agency.OwnerUserId);
                Assert.True(owner.IsManager);
                Assert.Equal(agency.Id, owner.AgencyId);

                var skippers = users.Where(u=>u.IsEmployee && u.AgencyId == agency.Id).ToList();
                Assert.Equal(2, skippers.Count);
            }
        }

        [Fact]
        public async Task SeedAsync_CreatesAdminAndGuestsWithoutAgency()
        {
            using var context = GetInMemoryDbContext();

            await DynamicDataSeeder.SeedAsync(context);

            var admin = await context.Users.SingleAsync(u => u.Email == "admin@rafting.local");
            Assert.True(admin.IsAdmin);
            Assert.Null(admin.AgencyId);

            var guest = await context.Users.SingleAsync(u => u.Email == "nina.bijedic@email.com");
            Assert.False(guest.IsAdmin || guest.IsManager || guest.IsEmployee);
            Assert.Null(guest.AgencyId);
            Assert.False(string.IsNullOrWhiteSpace(guest.Phone)); // the phone is required for guests
        }

        [Fact]
        public async Task SeedAsync_RunTwice_DoesNotCreateDuplicates()
        {
            using var context = GetInMemoryDbContext();

            await DynamicDataSeeder.SeedAsync(context);
            var users = await context.Users.CountAsync();
            var agencies = await context.Agencies.CountAsync();
            var rates = await context.ExchangeRates.CountAsync();

            await DynamicDataSeeder.SeedAsync(context);

            Assert.Equal(users, await context.Users.CountAsync());
            Assert.Equal(agencies, await context.Agencies.CountAsync());
            Assert.Equal(rates, await context.ExchangeRates.CountAsync());
        }

        [Fact]
        public async Task SeedAsync_WhenOneUserAlreadyExists_ReusesItAndAddsTheRest()
        {
            // The old seeder skipped everything if any user existed; the new one checks each entity.
            using var context = GetInMemoryDbContext();
            context.Users.Add(new RaftingUserEntity
            {
                Firstname = "Old",
                Lastname = "Manager",
                Email = "manager@rafting.local",
                IsManager = true
            });
            await context.SaveChangesAsync();

            await DynamicDataSeeder.SeedAsync(context);

            var manager = await context.Users.SingleAsync(u => u.Email == "manager@rafting.local");
            Assert.NotNull(manager.AgencyId); // the existing user was attached to the agency
            Assert.Equal(2, await context.Agencies.CountAsync());
        }
    }
}
