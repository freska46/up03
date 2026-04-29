using System.Text.Json.Serialization;

namespace PakDzal_Games_WPF.Models;

public class Game
{
    [JsonPropertyName("gameId")]
    public int GameId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("genre")]
    public string Genre { get; set; } = string.Empty;

    [JsonPropertyName("pricePerHour")]
    public decimal PricePerHour { get; set; }

    [JsonPropertyName("available")]
    public bool Available { get; set; }
}