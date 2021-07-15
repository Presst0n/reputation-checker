using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepDataCollector.Models
{
    public class Character
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("realm")]
        public Realm Realm { get; set; }

        [JsonProperty("playable_class")]
        public PlayableClass PlayableClass { get; set; }

        [JsonProperty("playable_race")]
        public PlayableRace PlayableRace { get; set; }
    }
}
