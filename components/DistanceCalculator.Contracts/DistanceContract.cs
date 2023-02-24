using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DistanceCalculator.Contracts
{
    /// <summary>
    /// distance between iata codes response body
    /// </summary>
    public sealed class DistanceContract
    {
        public DistanceContract(string originIata, string destinationIata, double distance, string unit)
        {
            OriginIata = originIata;
            DestinationIata = destinationIata;
            Distance = distance;
            Unit = unit;
        }

        /// <summary>
        /// origin iata code
        /// </summary>
        [Required]
        [JsonProperty("originIata")]
        public string OriginIata { get; }

        /// <summary>
        /// destination iata code
        /// </summary>
        [Required]
        [JsonProperty("destinationIata")]
        public string DestinationIata { get; }

        /// <summary>
        /// distance value
        /// </summary>
        [Required]
        [JsonProperty("distance")]
        public double Distance { get; }

        /// <summary>
        /// unit of distance calculated (mi, km)
        /// </summary>
        [Required]
        [StringLength(2)]
        [JsonProperty("unit")]
        public string Unit { get; }
    }
}