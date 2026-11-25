using Microsoft.Extensions.DependencyInjection;
using Rafting.Application.Abstractions;
using Rafting.Domain.Entities.Identity;
using Rafting.Domain.Entities.Notifications;

namespace Rafting.Tests.NotificationTests.IntegrationTests;

[Collection(IntegrationTestCollection.Name)]
public class NotificationServiceIntegrationTests
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public NotificationServiceIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task NotificationService_FromContainer_SavesWithHandlerAndSendsEmail()
    {
        // Arrange: one DI scope = one "request"
        using var scope = _factory.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
        var notifications = scope.ServiceProvider.GetRequiredService<INotificationService>();

        var email = $"{Guid.NewGuid():N}@test.local";
        var user = new RaftingUserEntity { Firstname = "Test", Lastname = "User", Email = email };
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync(default);

        // Act: the service adds, the "handler" (this test) saves
        await notifications.NotifyAsync([user.Id], new NotificationContent(NotificationType.General, "Hello", "World"));
        await ctx.SaveChangesAsync(default);

        // Assert
        Assert.Single(await ctx.Notifications.Where(n => n.UserId == user.Id).ToListAsync());
        Assert.Single(_factory.Emails.SentTo(email));
    }

    [Fact]
    public async Task WeatherAdvisory_FromContainer_ReturnsAnEntryForEveryDay()
    {
        using var scope = _factory.Services.CreateScope();
        var advisory = scope.ServiceProvider.GetRequiredService<IWeatherAdvisory>();

        var result = await advisory.GetRiskAsync(1, new DateOnly(2030, 1, 1), new DateOnly(2030, 1, 3));

        Assert.Equal(3, result.Count);
    }
}