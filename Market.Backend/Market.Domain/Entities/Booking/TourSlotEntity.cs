using Market.Domain.Common;

namespace Market.Domain.Entities.Booking;

/// <summary>
/// Termin ture (konkretan datum i sat). KOSTUR: puni ga tok B (generisanje termina, raspored).
/// Tok A ga samo čita.
/// </summary>
public class TourSlotEntity : BaseEntity
{
    // Strani ključ prema Tour dodaje tok B (B3) kad entitet Tour postoji.
    public int TourId { get; set; }

    public DateTime StartsAtUtc { get; set; }
    public int Capacity { get; set; }
    public TourSlotStatus Status { get; set; } = TourSlotStatus.Open;
    public string? CancelReason { get; set; }

    public ICollection<ReservationEntity> Reservations { get; private set; } = new List<ReservationEntity>();

    public static class Constraints
    {
        public const int CancelReasonMaxLength = 300;
    }
}