using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DistanceCalculator.ValueObjects;

namespace DistanceCalculator.Core.ValueObjects;

/// <summary>
/// create the instance of <see cref="Location"/>
/// entity inherits from value object
/// </summary>
public class Location : ValueObject
{
    [Required]
    [JsonPropertyName("lon")]
    public double Lon { get; set; }

    [Required]
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Lon;
        yield return Lat;
    }
}