namespace EMS.Common {
    using System;
    using System.Text.Json.Serialization;

    [Serializable]
    public class Error {
        [JsonPropertyName("field")] public string Field { get; set; }
        [JsonPropertyName("message")] public string Message { get; set; }
    }
}