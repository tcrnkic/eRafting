using Rafting.Domain.Entities.Notifications;

namespace Rafting.Application.Abstractions;

/// <summary>Content of a notification. <c>Message</c> is plain text, not HTML. Length limits: <c>NotificationEntity.Constraints</c>.</summary>
public sealed record NotificationContent(
    NotificationType Type,
    string Title,
    string Message,
    string? RelatedEntityType = null,
    int? RelatedEntityId = null);