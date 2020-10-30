namespace EMS.Common {
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class ApiResult
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }

        [JsonPropertyName("data")]
        public List<User> Data { get; set; }
    }
}