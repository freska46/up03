using System.Text.Json.Serialization;

namespace PakDzal_Games_WPF.Models;

public class User
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; } = "Курск";

    [JsonPropertyName("registrationDate")]
    public DateTime RegistrationDate { get; set; }
}