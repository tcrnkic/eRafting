using Market.Domain.Common;
using Market.Domain.Entities.Finance;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities.Agencies;

/// <summary>
/// Agencija koja objavljuje ture na platformi. Svi podaci vlasnika i skipera
/// vežu se na agenciju preko AgencyId (multi-tenant).
/// </summary>
public class AgencyEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    /// <summary>Jedinstven dio adrese javne stranice: /a/{slug}.</summary>
    public string Slug { get; set; } = string.Empty;

    public AgencyStatus Status { get; set; } = AgencyStatus.Active;

    public int OwnerUserId { get; set; }
    public MarketUserEntity? Owner { get; set; }

    public int DefaultCurrencyId { get; set; }
    public CurrencyEntity? DefaultCurrency { get; set; }

    // ---- Postavke agencije (vlasnik ih mijenja u dashboardu) ----

    /// <summary>Razmak za pripremu između tura: 0, 15, 30 ili 60 minuta.</summary>
    public int PreparationBufferMinutes { get; set; } = 15;

    /// <summary>Gost može rezervisati najkasnije toliko sati prije početka ture.</summary>
    public int BookingCutoffHours { get; set; } = 24;

    /// <summary>Spajanje malih grupa: najviše toliko dana razmaka između termina.</summary>
    public int MergeWindowDays { get; set; } = 3;

    /// <summary>Spajanje: odluka mora pasti toliko sati prije ranijeg termina.</summary>
    public int MergeDecisionHours { get; set; } = 48;

    /// <summary>Prognoza: padavine (mm) iznad kojih se dan označava crveno.</summary>
    public decimal RainThresholdMm { get; set; } = 10m;

    /// <summary>Prognoza: vjetar (km/h) iznad kojeg se dan označava crveno.</summary>
    public decimal WindThresholdKmh { get; set; } = 40m;

    /// <summary>Prognoza: temperatura (°C) ispod koje se dan označava crveno.</summary>
    public decimal MinTempC { get; set; } = 5m;

    public static class Constraints
    {
        public const int NameMaxLength = 100;
        public const int SlugMaxLength = 60;
    }
}