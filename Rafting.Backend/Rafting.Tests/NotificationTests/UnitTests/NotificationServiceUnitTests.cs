using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Rafting.Application.Abstractions;
using Rafting.Domain.Entities.Identity;
using Rafting.Domain.Entities.Notifications;
using Rafting.Infrastructure.Notifications;

namespace Rafting.Tests.NotificationTests.UnitTests;

public class NotificationServiceUnitTests
{
    private readonly FakeTimeProvider _clock = new(new DateTimeOffset(2026, 10, 19, 12, 0, 0, TimeSpan.Zero));

    private DatabaseContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // each test gets a new database
            .Options;

        var context = new DatabaseContext(options, _clock);
        context.Database.EnsureCreated();
        return context;
    }

    private static RaftingUserEntity NewUser(string email) =>
        new() { Firstname = "Test", Lastname = "User", Email = email };

    private static NotificationContent NewContent() =>
        new(NotificationType.General, "Title", "Message");

    private static NotificationService NewService(DatabaseContext context, IEmailSender? emailSender = null) =>
    new(context, emailSender ?? new CapturingEmailSender(), NullLogger<NotificationService>.Instance);

    private sealed class FailingEmailSender : IEmailSender
    {
        public Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default) =>
            Task.FromException(new InvalidOperationException("SMTP is down"));
    }

    [Fact]
    public async Task NotifyAsync_AddsNotificationsToContext_WithoutSaving()
    {
        using var context = GetInMemoryDbContext();
        var user1 = NewUser("one@test.local");
        var user2 = NewUser("two@test.local");
        context.Users.AddRange(user1, user2);
        await context.SaveChangesAsync();
        var service = NewService(context);

        await service.NotifyAsync([user1.Id, user2.Id], NewContent());

        // Tracked as Added, but not in the database yet: the calling handler saves.
        Assert.Equal(2, context.ChangeTracker.Entries<NotificationEntity>()
            .Count(e => e.State == EntityState.Added));
        Assert.Empty(await context.Notifications.ToListAsync());
    }



    [Fact]
    public async Task NotifyAsync_AfterSave_StoresContent()
    {
        using var context = GetInMemoryDbContext();
        var user = NewUser("one@test.local");
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var service = NewService(context);

        await service.NotifyAsync(
            [user.Id],
            new NotificationContent(NotificationType.ReservationConfirmed, "Confirmed", "Your reservation is confirmed.", "Reservation", 42));
        await context.SaveChangesAsync();

        var saved = await context.Notifications.SingleAsync();
        Assert.Equal(user.Id, saved.UserId);
        Assert.Equal(NotificationType.ReservationConfirmed, saved.Type);
        Assert.Equal("Confirmed", saved.Title);
        Assert.Equal("Your reservation is confirmed.", saved.Message);
        Assert.Equal("Reservation", saved.RelatedEntityType);
        Assert.Equal(42, saved.RelatedEntityId);
        Assert.False(saved.IsRead);
    }

    [Fact]
    public async Task NotifyAsync_DuplicateAndUnknownIds_AreIgnored()
    {
        using var context = GetInMemoryDbContext();
        var user = NewUser("one@test.local");
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var service = NewService(context);

        await service.NotifyAsync([user.Id, user.Id, 99999], NewContent());
        await context.SaveChangesAsync();

        Assert.Single(await context.Notifications.ToListAsync());
    }

    [Fact]
    public async Task NotifyAsync_EmptyList_DoesNothing()
    {
        using var context = GetInMemoryDbContext();
        var service = NewService(context);

        await service.NotifyAsync([], NewContent());
        await context.SaveChangesAsync();

        Assert.Empty(await context.Notifications.ToListAsync());
    }

    [Fact]
    public async Task NotifyAsync_DisabledUser_IsSkipped()
    {
        using var context = GetInMemoryDbContext();
        var active = NewUser("active@test.local");
        var disabled = NewUser("disabled@test.local");
        disabled.IsEnabled = false;
        context.Users.AddRange(active, disabled);
        await context.SaveChangesAsync();
        var service = NewService(context);

        await service.NotifyAsync([active.Id, disabled.Id], NewContent());
        await context.SaveChangesAsync();

        var saved = await context.Notifications.SingleAsync();
        Assert.Equal(active.Id, saved.UserId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task NotifyAsync_EmptyTitle_Throws(string title)
    {
        using var context = GetInMemoryDbContext();
        var service = NewService(context);
        var content = new NotificationContent(NotificationType.General, title, "Message");

        await Assert.ThrowsAsync<ArgumentException>(() => service.NotifyAsync([1], content));
    }

    [Fact]
    public async Task NotifyAsync_TooLongTitle_Throws()
    {
        using var context = GetInMemoryDbContext();
        var service = NewService(context);
        var title = new string('x', NotificationEntity.Constraints.TitleMaxLength + 1);
        var content = new NotificationContent(NotificationType.General, title, "Message");

        await Assert.ThrowsAsync<ArgumentException>(() => service.NotifyAsync([1], content));
    }

    [Fact]
    public async Task NotifyAsync_TooLongMessage_Throws()
    {
        using var context = GetInMemoryDbContext();
        var service = NewService(context);
        var message = new string('x', NotificationEntity.Constraints.MessageMaxLength + 1);
        var content = new NotificationContent(NotificationType.General, "Title", message);

        await Assert.ThrowsAsync<ArgumentException>(() => service.NotifyAsync([1], content));
    }

    [Fact]
    public async Task NotifyAsync_TitleAndMessageAtLimit_AreAccepted()
    {
        using var context = GetInMemoryDbContext();
        var user = NewUser("one@test.local");
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var service = NewService(context);
        var content = new NotificationContent(
            NotificationType.General,
            new string('x', NotificationEntity.Constraints.TitleMaxLength),
            new string('x', NotificationEntity.Constraints.MessageMaxLength));

        await service.NotifyAsync([user.Id], content);
        await context.SaveChangesAsync();

        Assert.Single(await context.Notifications.ToListAsync());
    }

    [Fact]
    public async Task NotifyAsync_SendsEmailToEnabledUser_WithTitleAsSubject()
    {
        using var context = GetInMemoryDbContext();
        var user = NewUser("one@test.local");
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var emails = new CapturingEmailSender();
        var service = NewService(context, emails);

        await service.NotifyAsync([user.Id], new NotificationContent(NotificationType.General, "Hello", "World"));

        var sent = Assert.Single(emails.SentTo("one@test.local"));
        Assert.Equal("Hello", sent.Subject);
        Assert.Contains("World", sent.HtmlBody);
    }

    [Fact]
    public async Task NotifyAsync_DisabledUser_GetsNoEmail()
    {
        using var context = GetInMemoryDbContext();
        var disabled = NewUser("disabled@test.local");
        disabled.IsEnabled = false;
        context.Users.Add(disabled);
        await context.SaveChangesAsync();
        var emails = new CapturingEmailSender();
        var service = NewService(context, emails);

        await service.NotifyAsync([disabled.Id], NewContent());

        Assert.Empty(emails.SentTo("disabled@test.local"));
    }

    [Fact]
    public async Task NotifyAsync_EncodesHtmlInEmailBody()
    {
        using var context = GetInMemoryDbContext();
        var user = NewUser("one@test.local");
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var emails = new CapturingEmailSender();
        var service = NewService(context, emails);
        var content = new NotificationContent(
            NotificationType.General, "<b>Title</b>", "<a href=\"https://evil.test\">click</a>");

        await service.NotifyAsync([user.Id], content);

        var body = Assert.Single(emails.SentTo("one@test.local")).HtmlBody;
        Assert.DoesNotContain("<a href", body);
        Assert.DoesNotContain("<b>", body);
        Assert.Contains("&lt;a href", body);
    }

    [Fact]
    public async Task NotifyAsync_EmailFails_DoesNotThrowAndStillAddsNotification()
    {
        using var context = GetInMemoryDbContext();
        var user = NewUser("one@test.local");
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var service = NewService(context, new FailingEmailSender());

        await service.NotifyAsync([user.Id], NewContent()); // must not throw
        await context.SaveChangesAsync();

        Assert.Single(await context.Notifications.ToListAsync());
    }
}