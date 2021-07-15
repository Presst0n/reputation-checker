using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RepDataCollector.Models
{
    public class Realm
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("Slug")]
        public string Slug { get; set; }
    }
}
