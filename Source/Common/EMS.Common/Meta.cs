namespace EMS.Common {
    using System.Text.Json.Serialization;

    public class Meta
    {
        [JsonPropertyName("pagination")]
        public Pagination Pagination { get; set; }
    }
}