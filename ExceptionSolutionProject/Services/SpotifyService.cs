using System.Net.Http;
using System.Threading.Tasks;

public class SpotifyService
{
    private readonly HttpClient _httpClient;

    public SpotifyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> SearchMusicAsync(string query)
    {
        var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/search?q={query}&type=track");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
