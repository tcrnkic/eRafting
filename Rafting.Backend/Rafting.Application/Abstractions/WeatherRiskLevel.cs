namespace Rafting.Application.Abstractions;

/// <summary>Weather risk of one day. <c>Unknown</c> = 0 is the safe default: missing data is never reported as fine.</summary>
public enum WeatherRiskLevel
{
    /// <summary>No forecast (not available or beyond the horizon). Not the same as <c>Normal</c>.</summary>
    Unknown = 0,
    Normal = 1,
    /// <summary>An agency threshold (rain, wind, temperature) is exceeded. It only warns; the owner cancels manually.</summary>
    Red = 2
}