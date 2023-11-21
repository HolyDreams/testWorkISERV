using System.Text.Json.Serialization;

namespace testWorkISERV.Models
{
    public class DTOData
    {
        [JsonPropertyName("country")]
        public string CountryName { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("web_pages")]
        public string? WebPages { get; set; }
    }
}
