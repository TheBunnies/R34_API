using Newtonsoft.Json;
using System.Collections.Generic;

namespace R34_API.Models
{
    [JsonObject]
    public class Response
    {
        [JsonProperty("items")]
        public List<Item> Items {get; set;}
        [JsonProperty("maxPages")]
        public int MaxPages {get; set;}
    }
}
