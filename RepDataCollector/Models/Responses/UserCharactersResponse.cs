using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RepDataCollector.Models.Responses
{
    public class UserCharactersResponse
    {
        [JsonProperty("wow_accounts")]
        public WowAccount[] WowAccounts { get; set; }
    }
}
