using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class AiService : IAiService
{
    private readonly IConfiguration _config;

    public AiService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<string?> GetResponseFromModel(string prompt)
    {
        var url = _config["GitHubModels:BaseUrl"];
        var modelName = _config["GitHubModels:ModelName"];
        var apiKey = _config["GitHubModels:Token"];

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("CrmApp");

        var body = new StringContent(
            JsonSerializer.Serialize(new
            {
                model = modelName,
                messages = new[] { new { role = "user", content = prompt } },
                max_tokens = 1000
            }),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PostAsync(url, body);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao chamar o modelo '{modelName}': {response.StatusCode}. Detalhes: {error}");
        }

        using var jsonDoc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return jsonDoc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();
    }
}
