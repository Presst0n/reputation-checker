using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RepDataCollector.Models
{
    public class WowAccount
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("characters")]
        public Character[] Characters { get; set; }

    }
}
