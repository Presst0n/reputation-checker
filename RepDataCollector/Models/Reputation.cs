using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepDataCollector.Models
{
    public class Reputation
    {
        [JsonProperty("Faction")]
        public Faction Faction { get; set; }

        [JsonProperty("Standing")]
        public Standing Standing { get; set; }
    }
}
