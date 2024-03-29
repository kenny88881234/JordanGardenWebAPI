using System.Text.Json.Serialization;

namespace JordanGardenStockWebAPI.Models;

public class APIResult<T>
{
    [JsonPropertyName("succ")]
    public bool Succ { get; set; }
    [JsonPropertyName("errorCode")]
    public string? ErrorCode { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}