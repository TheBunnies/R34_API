using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace R34_API.Models
{
    [JsonObject]
    public class Item
    {
        [JsonProperty("tags")]
        public string Tags {get; set;}
        [JsonProperty("url")]
        public string Url {get; set;}
        [JsonProperty("width")]
        public int Width {get; set;}
        [JsonProperty("height")]
        public int Height {get; set;}
        [JsonProperty("type")]
        [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
        public Type Type {get; set;}
    }
    public enum Type
    {
        Video,
        Image
    }
}
