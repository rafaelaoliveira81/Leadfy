using System.Text.Json;

public class AiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
 
    public AiService(IConfiguration config)
    {
        _httpClient = new HttpClient();
        _config = config;
    }

    public async Task<string> GetAiResponseAsync(string prompt)
    {
        var url = _config["GitHubModels:BaseUrl"];
        var token = _config["GitHubModels:Token"];

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Itera360App");

        var content = new StringContent(
            JsonSerializer.Serialize(new { prompt = prompt, max_tokens = 100 }),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            throw new Exception($"Erro: {response.StatusCode}, Detalhes: {error}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }
}