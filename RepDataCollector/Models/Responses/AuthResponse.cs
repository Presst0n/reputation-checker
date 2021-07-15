using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepDataCollector.Models.Responses
{
    public class AuthResponse
    {
        [JsonProperty("Access_Token")]
        public string Access_Token { get; set; }
    }
}
