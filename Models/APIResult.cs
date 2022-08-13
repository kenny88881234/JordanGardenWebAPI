using System.Text.Json.Serialization;

public class APIResult<T>
{
    [JsonPropertyName("succ")]
    public bool Succ { get; set; }
    [JsonPropertyName("errorCode")]
    public string? ErrorCode { get; set; }
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}