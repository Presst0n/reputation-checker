using Newtonsoft.Json;

namespace RepDataCollector.Models
{
    public class Standing
    {
        [JsonProperty("raw")]
        public int Raw { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("max")]
        public int Max { get; set; }

        [JsonProperty("tier")]
        public int Tier { get; set; }
    }
}