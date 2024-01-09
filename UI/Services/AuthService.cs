using UI.Dto;

namespace UI.Services;

public class AuthService
{
    private Dictionary<string, string> _tokens = new Dictionary<string, string>();

    private HttpClient _httpClient;

    public AuthService(IConfiguration configuration)
    {
        var apiUrl = configuration["ApiUrl"];
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(apiUrl ?? "")
        };
    }

    public async Task<string> GetTokenAsync(LoginDto loginDto)
    {
        if (_tokens.TryGetValue(loginDto.Username, out var token))
            return token;
        var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/login", loginDto);
        if (!response.IsSuccessStatusCode)
            throw new Exception("Could not log on");
        token = await response.Content.ReadAsStringAsync();
        _tokens[loginDto.Username] = token;
        return token;
    }
}