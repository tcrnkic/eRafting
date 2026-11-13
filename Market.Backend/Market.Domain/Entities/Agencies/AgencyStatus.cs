namespace Market.Domain.Entities.Agencies;

/// <summary>
/// Stanje agencije na platformi.
/// </summary>
public enum AgencyStatus
{
    /// <summary>Prijava je odobrena i agencija radi.</summary>
    Active = 1,

    /// <summary>Admin je agenciju suspendovao: nema javne stranice.</summary>
    Suspended = 2
}