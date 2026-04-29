using System.Text.Json.Serialization;

namespace PakDzal_Games_WPF.Models;

public class Session
{
    [JsonPropertyName("sessionId")]
    public int SessionId { get; set; }

    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("user")]
    public User? User { get; set; }

    [JsonPropertyName("gameId")]
    public int GameId { get; set; }

    [JsonPropertyName("game")]
    public Game? Game { get; set; }

    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public DateTime? EndTime { get; set; }

    [JsonPropertyName("totalPrice")]
    public decimal? TotalPrice { get; set; }
}