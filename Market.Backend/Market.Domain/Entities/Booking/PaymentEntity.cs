using Market.Domain.Common;
using Market.Domain.Entities.Finance;

namespace Market.Domain.Entities.Booking;

/// <summary>
/// Uplata ili povrat uz rezervaciju. KOSTUR: logiku puni tok A (uplate i valute).
/// </summary>
public class PaymentEntity : BaseEntity
{
    public int ReservationId { get; set; }
    public ReservationEntity? Reservation { get; set; }

    public int CurrencyId { get; set; }
    public CurrencyEntity? Currency { get; set; }

    public PaymentType Type { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    /// <summary>Iznos u valuti uplate.</summary>
    public decimal Amount { get; set; }

    /// <summary>Kurs prema KM, datum kursa i iznos preračunat u KM.</summary>
    public decimal RateToBase { get; set; }
    public DateTime RateDate { get; set; }
    public decimal AmountInBase { get; set; }

    public DateTime? PaidAtUtc { get; set; }
}