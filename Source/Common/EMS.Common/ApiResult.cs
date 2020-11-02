namespace EMS.Common {
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class ApiResult<T>
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }
    }

    public class Error {
        [JsonPropertyName("field")]
        public string Field { get; set; }
        [JsonPropertyName("message")]

        public string Message { get; set; }
    }

    public class ErrorConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(IEnumerable<Error>);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return Activator.CreateInstance<ErrorConverter>();
        }
    }
    public class ErrorConverter : JsonConverter<IEnumerable<Error>>
    {
        public override IEnumerable<Error> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return  null;
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<Error> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}