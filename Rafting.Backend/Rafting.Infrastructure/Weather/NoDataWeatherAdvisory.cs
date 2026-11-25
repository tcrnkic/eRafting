using Rafting.Application.Abstractions;

namespace Rafting.Infrastructure.Weather;

/// <summary>
/// Temporary <see cref="IWeatherAdvisory"/> until flow B delivers the real forecast.
/// There is no data, so every day is <c>Unknown</c>.
/// </summary>
public sealed class NoDataWeatherAdvisory : IWeatherAdvisory
{
    public Task<IReadOnlyDictionary<DateOnly, WeatherRiskLevel>> GetRiskAsync(
        int agencyId, DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        var result = new Dictionary<DateOnly, WeatherRiskLevel>();

        for (var day = from; day <= to; day = day.AddDays(1))
        {
            result[day] = WeatherRiskLevel.Unknown;
        }

        return Task.FromResult<IReadOnlyDictionary<DateOnly, WeatherRiskLevel>>(result);
    }
}