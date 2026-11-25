using System.Net;
using Microsoft.Extensions.Logging;
using Rafting.Application.Abstractions;
using Rafting.Domain.Entities.Notifications;

namespace Rafting.Infrastructure.Notifications;

/// <summary>Implementation of <see cref="INotificationService"/>; the rules are described on the interface.</summary>
public sealed class NotificationService(
    IAppDbContext ctx, IEmailSender emailSender, ILogger<NotificationService> logger) : INotificationService
{
    public async Task NotifyAsync(
        IReadOnlyCollection<int> userIds, NotificationContent content, CancellationToken ct = default)
    {
        ValidateContent(content);

        var ids = userIds.Distinct().ToList();
        if (ids.Count == 0)
            return;

        // One query for all recipients (not one per user). Deleted users are skipped
        // by the global query filter, so unknown and deleted ids are both missing here.
        var users = await ctx.Users
            .AsNoTracking()
            .Where(u => ids.Contains(u.Id))
            .Select(u => new { u.Id, u.Email, u.IsEnabled })
            .ToListAsync(ct);

        var unknownIds = ids.Except(users.Select(u => u.Id)).ToList();
        if (unknownIds.Count > 0)
            logger.LogWarning("Notification skipped for unknown or deleted user ids: {UserIds}", string.Join(", ", unknownIds));

        // The same for every recipient, so it is built once. The text is user-influenced,
        // so it is HTML-encoded (IEmailSender requires the caller to do it).
        var htmlBody = $"<h3>{WebUtility.HtmlEncode(content.Title)}</h3><p>{WebUtility.HtmlEncode(content.Message)}</p>";

        foreach (var user in users.Where(u => u.IsEnabled))
        {
            ctx.Notifications.Add(new NotificationEntity
            {
                UserId = user.Id,
                Type = content.Type,
                Title = content.Title,
                Message = content.Message,
                RelatedEntityType = content.RelatedEntityType,
                RelatedEntityId = content.RelatedEntityId
            });

            await TrySendEmailAsync(user.Id, user.Email, content.Title, htmlBody, ct);
        }

        // No SaveChangesAsync on purpose: the calling handler saves, so the notifications
        // are committed in the same transaction as the business change.
    }

    // A failed email must not abort the business operation, so it is only logged.
    // Cancellation is not a failure and is passed on. Only the user id is logged, not the address.
    private async Task TrySendEmailAsync(int userId, string email, string subject, string htmlBody, CancellationToken ct)
    {
        try
        {
            await emailSender.SendAsync(email, subject, htmlBody, ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "Sending the notification email to user {UserId} failed.", userId);
        }
    }

    // Invalid content is a programming error, so we fail fast instead of truncating.
    private static void ValidateContent(NotificationContent content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content.Title);
        ArgumentException.ThrowIfNullOrWhiteSpace(content.Message);

        if (content.Title.Length > NotificationEntity.Constraints.TitleMaxLength)
            throw new ArgumentException(
                $"Title is longer than {NotificationEntity.Constraints.TitleMaxLength} characters.", nameof(content));

        if (content.Message.Length > NotificationEntity.Constraints.MessageMaxLength)
            throw new ArgumentException(
                $"Message is longer than {NotificationEntity.Constraints.MessageMaxLength} characters.", nameof(content));
    }
}