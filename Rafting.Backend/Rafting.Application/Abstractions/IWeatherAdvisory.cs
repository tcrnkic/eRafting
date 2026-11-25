namespace Rafting.Application.Abstractions;

/// <summary>Weather risk per day for an agency. Flow A reads it, flow B implements it.</summary>
public interface IWeatherAdvisory
{
    /// <summary>
    /// Risk for every day in [from, to], both inclusive. Days without a forecast are <c>Unknown</c> (never an exception).
    /// One call for the whole range, so an implementation can make one request instead of one per day.
    /// </summary>
    Task<IReadOnlyDictionary<DateOnly, WeatherRiskLevel>> GetRiskAsync(
        int agencyId, DateOnly from, DateOnly to, CancellationToken ct = default);
}