namespace EMS.Integration {
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Common;

    public class ApiResult<T> {
        private List<Error> errors;
        private T data;
        [JsonPropertyName("code")] public int Code { get; set; }

        [JsonPropertyName("meta")] public Meta Meta { get; set; }
        [JsonExtensionData] public IDictionary<string, JsonElement> Extras { get; set; }

        [JsonIgnore]
        public T Data {
            get {
                if (data != null)
                    return data;
                if (Extras != null && Extras.TryGetValue("data", out var element)) {
                    try {
                        data = JsonSerializer.Deserialize<T>(element.GetRawText());
                    }
                    catch {
                        // ignored
                    }
                }

                return data;
            }
        }

        [JsonIgnore]
        public IEnumerable<Error> Errors {
            get {
                if (errors != null)
                    return errors;
                if (Extras == null || !Extras.TryGetValue("data", out var element)) return errors;
                
                errors = element.ValueKind == JsonValueKind.Object
                    ? new List<Error> {JsonSerializer.Deserialize<Error>(element.GetRawText())}
                    : JsonSerializer.Deserialize<List<Error>>(element.GetRawText());
                errors.RemoveAll(error => string.IsNullOrWhiteSpace(error.Field) && string.IsNullOrWhiteSpace(error.Message));

                return errors;
            }
        }
    }
}