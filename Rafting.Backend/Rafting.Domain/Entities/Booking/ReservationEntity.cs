using Rafting.Domain.Common;
using Rafting.Domain.Entities.Finance;
using Rafting.Domain.Entities.Identity;

namespace Rafting.Domain.Entities.Booking;

/// <summary>
/// Rezervacija gosta za termin. KOSTUR: logiku (potvrda, otkazivanje, spajanje) puni tok A.
/// Tok B je samo čita (kalendar, dashboard).
/// </summary>
public class ReservationEntity : BaseEntity
{
    public int GuestUserId { get; set; }
    public RaftingUserEntity? GuestUser { get; set; }

    public int TourSlotId { get; set; }
    public TourSlotEntity? TourSlot { get; set; }

    public int CurrencyId { get; set; }
    public CurrencyEntity? Currency { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;
    public int NumberOfPeople { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public DateTime ExpiresAtUtc { get; set; }

    /// <summary>Ukupan iznos u valuti rezervacije.</summary>
    public decimal Total { get; set; }

    /// <summary>Kurs prema KM u trenutku rezervacije i datum tog kursa.</summary>
    public decimal RateToBase { get; set; }
    public DateTime RateDate { get; set; }

    public ICollection<PaymentEntity> Payments { get; private set; } = new List<PaymentEntity>();

    public static class Constraints
    {
        public const int ReferenceNumberMaxLength = 20;
    }
}