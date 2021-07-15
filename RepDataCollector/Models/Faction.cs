using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepDataCollector.Models
{
    public class Faction
    {
        [JsonProperty("Key")]
        public Key Key { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Id")]
        public uint Id { get; set; }
    }
}
