using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RepDataCollector.Models.Responses
{
    public class ReputationResponse
    {
        [JsonProperty("character")]
        public Character Character { get; set; }

        [JsonProperty("reputations")]
        public List<Reputation> Reputations { get; set; }
    }
}
