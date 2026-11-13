using Market.Domain.Common;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities.Notifications;

/// <summary>
/// Obavijest korisniku (gost, vlasnik agencije ili skiper). Upisuje je INotificationService.
/// </summary>
public class NotificationEntity : BaseEntity
{
    public int UserId { get; set; }
    public MarketUserEntity? User { get; set; }

    public NotificationType Type { get; set; } = NotificationType.General;

    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; }
    public DateTime? ReadAtUtc { get; set; }

    /// <summary>Opcionalno: na šta se obavijest odnosi, npr. "Reservation" i Id rezervacije.</summary>
    public string? RelatedEntityType { get; set; }
    public int? RelatedEntityId { get; set; }

    public static class Constraints
    {
        public const int TitleMaxLength = 150;
        public const int MessageMaxLength = 1000;
        public const int RelatedEntityTypeMaxLength = 50;
    }
}