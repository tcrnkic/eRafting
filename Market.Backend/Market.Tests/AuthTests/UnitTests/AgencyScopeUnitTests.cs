using Market.Application.Abstractions;
using Market.Application.Common;
using Market.Domain.Common;

namespace Market.Tests.AuthTests.UnitTests;

public class AgencyScopeUnitTests
{
    private sealed record Item(int Id, int AgencyId) : IAgencyOwned;

    private sealed class User(bool isAdmin, bool isManager, bool isEmployee, int? agencyId) : IAppCurrentUser
    {
        public int? UserId => 1;
        public string? Email => null;
        public bool IsAuthenticated => true;
        public bool IsAdmin => isAdmin;
        public bool IsManager => isManager;
        public bool IsEmployee => isEmployee;
        public int? AgencyId => agencyId;
    }

    private static readonly IQueryable<Item> Items = new[]
    {
        new Item(1, AgencyId: 1),
        new Item(2, AgencyId: 1),
        new Item(3, AgencyId: 2)
    }.AsQueryable();

    [Theory]
    [InlineData(true, false)]  // owner
    [InlineData(false, true)]  // employee (skipper)
    public void ScopedForRead_OwnerOrEmployee_SeesOnlyOwnAgency(bool isManager, bool isEmployee)
    {
        var user = new User(false, isManager, isEmployee, agencyId: 1);

        var result = Items.ScopedForRead(user).Select(x => x.Id).ToList();

        Assert.Equal([1, 2], result);
    }

    [Fact]
    public void ScopedForRead_Admin_SeesAllAgencies()
    {
        var user = new User(true, false, false, agencyId: null);

        Assert.Equal(3, Items.ScopedForRead(user).Count());
    }

    [Fact]
    public void ScopedToOwnAgency_Admin_HasNoBypass()
    {
        var user = new User(true, false, false, agencyId: null);

        Assert.Empty(Items.ScopedToOwnAgency(user));
    }

    [Fact]
    public void ScopedForRead_GuestOrManagerWithoutAgency_SeesNothing()
    {
        var guest = new User(false, false, false, agencyId: null);
        var managerWithoutAgency = new User(false, true, false, agencyId: null);

        Assert.Empty(Items.ScopedForRead(guest));
        Assert.Empty(Items.ScopedForRead(managerWithoutAgency));
    }
}