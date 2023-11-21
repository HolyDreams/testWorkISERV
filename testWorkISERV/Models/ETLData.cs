using System.Text.Json;
using System.Text.Json.Serialization;

namespace testWorkISERV.Models
{
    public class ETLData
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("state-province")]
        public string? Oblast { get; set; }
        [JsonPropertyName("web_pages")]
        public string[] WebPages { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("domains")]
        public string[] Domains { get; set; }
        [JsonPropertyName("alpha_two_code")]
        public string ShortName { get; set; }
    }
}
