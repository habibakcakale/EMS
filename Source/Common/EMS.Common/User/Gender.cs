namespace EMS.Common.User {
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender {
        Male,
        Female
    }
}