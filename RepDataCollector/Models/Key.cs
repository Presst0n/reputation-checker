using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RepDataCollector.Models
{
    public class Key
    {
        [JsonPropertyName("Href")]
        public string Href { get; set; }
    }
}
