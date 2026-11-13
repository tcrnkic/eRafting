// ReservationStatus.cs
namespace Market.Domain.Entities.Booking;

public enum ReservationStatus
{
    // aktivna stanja
    Pending = 1,
    Confirmed = 2,
    // završna stanja
    Rejected = 3,
    Expired = 4,
    CancelledByGuest = 5,
    CancelledByAgency = 6,
    Completed = 7
}