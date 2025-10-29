using System.Text.Json;

namespace Tests.Common;

public static class TestExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public static async Task<T> ToResponseModel<T>(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, SerializerOptions)
               ?? throw new InvalidOperationException("Response content cannot be null");
    }
}
