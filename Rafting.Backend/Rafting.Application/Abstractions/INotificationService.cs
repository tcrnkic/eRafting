namespace Rafting.Application.Abstractions;

/// <summary>
/// Notifies users. For each user it adds a notification and sends an email.
/// It does not save to the database: the handler that calls it does that with SaveChangesAsync,
/// so the notification is saved in the same transaction as the rest of the change.
/// Emails are sent right away. If sending fails, the error is logged and the operation continues.
/// Empty lists, duplicate ids, unknown ids and disabled users are ignored.
/// A title or message that is empty or too long throws ArgumentException.
/// </summary>
public interface INotificationService
{
    Task NotifyAsync(IReadOnlyCollection<int> userIds, NotificationContent content, CancellationToken ct = default);
}