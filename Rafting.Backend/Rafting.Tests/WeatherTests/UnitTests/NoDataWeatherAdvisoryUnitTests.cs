using Rafting.Application.Abstractions;
using Rafting.Infrastructure.Weather;

namespace Rafting.Tests.WeatherTests.UnitTests;

public class NoDataWeatherAdvisoryUnitTests
{
    private readonly IWeatherAdvisory _advisory = new NoDataWeatherAdvisory();

    [Fact]
    public async Task GetRiskAsync_ReturnsUnknownForEveryDayInRange()
    {
        var from = new DateOnly(2027, 6, 10);
        var to = new DateOnly(2027, 6, 14);

        var result = await _advisory.GetRiskAsync(agencyId: 1, from, to);

        Assert.Equal(5, result.Count);
        Assert.Contains(from, result.Keys);
        Assert.Contains(to, result.Keys);
        Assert.All(result.Values, risk => Assert.Equal(WeatherRiskLevel.Unknown, risk));
    }

    [Fact]
    public async Task GetRiskAsync_SingleDayRange_ReturnsOneEntry()
    {
        var day = new DateOnly(2027, 6, 10);

        var result = await _advisory.GetRiskAsync(agencyId: 1, day, day);

        Assert.Single(result);
        Assert.Equal(WeatherRiskLevel.Unknown, result[day]);
    }

    [Fact]
    public async Task GetRiskAsync_ToBeforeFrom_ReturnsEmptyResult()
    {
        var result = await _advisory.GetRiskAsync(
            agencyId: 1, new DateOnly(2027, 6, 14), new DateOnly(2027, 6, 10));

        Assert.Empty(result);
    }
}