using System;
using System.Text.Json.Serialization;

namespace AirCinelMVC.Data.Dtos
{
    public class UpdateFlightDateDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("newStart")]
        public DateTime NewStart { get; set; }

        [JsonPropertyName("newEnd")]
        public DateTime NewEnd { get; set; }
    }
}
