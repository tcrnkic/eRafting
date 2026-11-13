namespace Market.Domain.Entities.Notifications;

/// <summary>
/// Vrsta obavijesti. Tokovi A i B dodaju nove vrijednosti kad im zatrebaju.
/// </summary>
public enum NotificationType
{
    General = 1,
    ReservationCreated = 2,
    ReservationConfirmed = 3,
    ReservationRejected = 4,
    ReservationCancelled = 5,
    SlotCancelled = 6,
    MergeProposed = 7,
    AssignmentProposed = 8,
    AssignmentDeclined = 9
}