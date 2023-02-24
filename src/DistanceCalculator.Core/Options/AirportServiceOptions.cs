namespace DistanceCalculator.Core.Options;

public class AirportServiceOptions
{
    public const string SectionName = "AirportServiceOptions";

    public string Origin { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public int TimeoutInSeconds { get; set; } = 30;
}