namespace Market.Domain.Common;

/// <summary>
/// Marker for entities that belong to one agency (Tour, Boat, GuideProfile, ...).
/// Every new agency-owned entity implements this so the agency filter works on it.
/// </summary>
public interface IAgencyOwned
{
    int AgencyId { get; }
}