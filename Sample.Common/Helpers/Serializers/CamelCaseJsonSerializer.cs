using System.Text.Json;

namespace Sample.Common.Helpers.Serializers;

public class CamelCaseJsonSerializer {
    public static string Serialize(object value) {

        var serializerOptions = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(value, serializerOptions);
    }
}