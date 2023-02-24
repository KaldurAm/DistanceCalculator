using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DistanceCalculator.Core.ValueObjects;

namespace DistanceCalculator.Core.Contracts;

/// <summary>
/// entity presents the information by iata code from cteleport rest service
/// </summary>
public class Airport
{
    [Required]
    [StringLength(50)]
    [JsonPropertyName("country")]
    public string Country { get; set; } = null!;

    [Required]
    [StringLength(3)]
    [MinLength(3)]
    [JsonPropertyName("city_iata")]
    public string CityIata { get; set; } = null!;

    [Required]
    [StringLength(3)]
    [MinLength(3)]
    [JsonPropertyName("iata")]
    public string Iata { get; set; } = null!;

    [Required]
    [StringLength(50)]
    [JsonPropertyName("city")]
    public string City { get; set; } = null!;

    [Required]
    [StringLength(100)]
    [JsonPropertyName("timezone_region_name")]
    public string TimezoneRegionName { get; set; } = null!;

    [Required]
    [StringLength(2)]
    [MinLength(2)]
    [JsonPropertyName("country_iata")]
    public string CountryIata { get; set; } = null!;

    [Required]
    [JsonPropertyName("rating")]
    public int Rating { get; set; }

    [Required]
    [StringLength(50)]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [Required]
    [JsonPropertyName("location")]
    public Location Location { get; set; } = null!;

    [Required]
    [StringLength(50)]
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [Required]
    [JsonPropertyName("hubs")]
    public int Hubs { get; set; }
}