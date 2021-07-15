using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepDataCollector.Models.Responses
{
    public class FactionResponse
    {
        [JsonProperty("Name")]
        public Name Name { get; set; }
    }
}
