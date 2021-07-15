using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepDataCollector.Models
{
    public class Name
    {
        [JsonProperty("en_US")]
        public string English { get; set; }
    }
}
