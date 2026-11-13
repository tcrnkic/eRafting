using Market.Domain.Common;

namespace Market.Application.Common;

public static class AgencyScopeExtensions
{
    /// <summary>
    /// For READING: owner and employee see only their agency, the platform admin sees all agencies,
    /// everyone else (guest, anonymous) sees nothing.
    /// </summary>
    public static IQueryable<T> ScopedForRead<T>(this IQueryable<T> query, IAppCurrentUser user)
        where T : class, IAgencyOwned
    {
        if (user.IsAdmin) return query;
        return query.ScopedToOwnAgency(user);
    }

    /// <summary>
    /// For WRITING (and for owner/employee screens): only the user's own agency. The admin gets no bypass here.
    /// </summary>
    public static IQueryable<T> ScopedToOwnAgency<T>(this IQueryable<T> query, IAppCurrentUser user)
        where T : class, IAgencyOwned
    {
        if (user.AgencyId is int agencyId && (user.IsManager || user.IsEmployee))
            return query.Where(x => x.AgencyId == agencyId);

        return query.Where(_ => false);
    }
}