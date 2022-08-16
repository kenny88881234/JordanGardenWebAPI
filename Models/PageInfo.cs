using System.Text.Json.Serialization;

public class PageInfo
{
    [JsonPropertyName("dataNumPerPage")]
    public int DataNumPerPage { get; set; }
    [JsonPropertyName("totalDataNum")]
    public int TotalDataNum { get; set; }
    [JsonPropertyName("totalPageNum")]
    public int TotalPageNum { get; set; }
}