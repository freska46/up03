using System.Net.Http;
using System.Net.Http.Json;
using PakDzal_Games_WPF.Models;

namespace PakDzal_Games_WPF.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:5222/api";

    public ApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<User>> GetUsersAsync(string? search = null, string? city = null)
    {
        var url = $"{BaseUrl}/users";
        var queryParams = new List<string>();
        
        if (!string.IsNullOrEmpty(search))
            queryParams.Add($"search={Uri.EscapeDataString(search)}");
        if (!string.IsNullOrEmpty(city))
            queryParams.Add($"city={Uri.EscapeDataString(city)}");
        
        if (queryParams.Count > 0)
            url += "?" + string.Join("&", queryParams);

        return await _httpClient.GetFromJsonAsync<List<User>>(url) ?? new List<User>();
    }

    public async Task<User?> GetUserAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<User>($"{BaseUrl}/users/{id}");
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/users", user);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<User>())!;
    }

    public async Task UpdateUserAsync(User user)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/users/{user.UserId}", user);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteUserAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/users/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<object> GetUsersReportAsync(string? city = null)
    {
        var url = $"{BaseUrl}/users/report";
        if (!string.IsNullOrEmpty(city))
            url += $"?city={Uri.EscapeDataString(city)}";
        
        return await _httpClient.GetFromJsonAsync<object>(url) ?? new object();
    }

    public async Task<List<Game>> GetGamesAsync(string? search = null, string? genre = null, bool? available = null)
    {
        var url = $"{BaseUrl}/games";
        var queryParams = new List<string>();
        
        if (!string.IsNullOrEmpty(search))
            queryParams.Add($"search={Uri.EscapeDataString(search)}");
        if (!string.IsNullOrEmpty(genre))
            queryParams.Add($"genre={Uri.EscapeDataString(genre)}");
        if (available.HasValue)
            queryParams.Add($"available={available.Value}");
        
        if (queryParams.Count > 0)
            url += "?" + string.Join("&", queryParams);

        return await _httpClient.GetFromJsonAsync<List<Game>>(url) ?? new List<Game>();
    }

    public async Task<Game?> GetGameAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Game>($"{BaseUrl}/games/{id}");
    }

    public async Task<Game> CreateGameAsync(Game game)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/games", game);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<Game>())!;
    }

    public async Task UpdateGameAsync(Game game)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/games/{game.GameId}", game);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteGameAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/games/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<object> GetGamesReportAsync(bool? available = null)
    {
        var url = $"{BaseUrl}/games/report";
        if (available.HasValue)
            url += $"?available={available.Value}";
        
        return await _httpClient.GetFromJsonAsync<object>(url) ?? new object();
    }

    public async Task<List<Session>> GetSessionsAsync(int? userId = null, int? gameId = null)
    {
        var url = $"{BaseUrl}/sessions";
        var queryParams = new List<string>();
        
        if (userId.HasValue)
            queryParams.Add($"userId={userId.Value}");
        if (gameId.HasValue)
            queryParams.Add($"gameId={gameId.Value}");
        
        if (queryParams.Count > 0)
            url += "?" + string.Join("&", queryParams);

        return await _httpClient.GetFromJsonAsync<List<Session>>(url) ?? new List<Session>();
    }

    public async Task<Session?> GetSessionAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Session>($"{BaseUrl}/sessions/{id}");
    }

    public async Task<Session> CreateSessionAsync(Session session)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/sessions", session);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<Session>())!;
    }

    public async Task UpdateSessionAsync(Session session)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/sessions/{session.SessionId}", session);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteSessionAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/sessions/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<object> GetSessionsReportAsync(int? userId = null, int? gameId = null)
    {
        var url = $"{BaseUrl}/sessions/report";
        var queryParams = new List<string>();
        
        if (userId.HasValue)
            queryParams.Add($"userId={userId.Value}");
        if (gameId.HasValue)
            queryParams.Add($"gameId={gameId.Value}");
        
        if (queryParams.Count > 0)
            url += "?" + string.Join("&", queryParams);

        return await _httpClient.GetFromJsonAsync<object>(url) ?? new object();
    }
}