using Newtonsoft.Json;

namespace RepDataCollector.Models
{
    public class PlayableRace
    {
        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }
}