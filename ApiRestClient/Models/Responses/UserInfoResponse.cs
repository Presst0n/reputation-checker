using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepDataCollector.Models.Responses
{
    public class UserInfoResponse
    {
        [JsonProperty("BattleTag")]
        public string BattleTag { get; set; }
    }
}
