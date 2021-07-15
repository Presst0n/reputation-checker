using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RepDataCollector.Models
{
    public class UserCharacter
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("realm")]
        public Realm Realm { get; set; }
    }
}
