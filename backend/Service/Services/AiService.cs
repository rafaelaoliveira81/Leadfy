using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

/// <summary>
/// Implementação do serviço de IA usando a API GitHub Models.
/// </summary>
public class AiService : IAiService
{
    private readonly IConfiguration _config;

    public AiService(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Envia o <paramref name="prompt"/> ao modelo especificado usando a <paramref name="apiKey"/> fornecida.
    /// A URL base é lida de <c>GitHubModels:BaseUrl</c> nas configurações da aplicação.
    /// </summary>
    /// <exception cref="Exception">Lançada quando a API retorna um status de erro.</exception>
    public async Task<string?> GetResponseFromModel(string prompt, string modelName, string apiKey)
    {
        var url = _config["GitHubModels:BaseUrl"]
            ?? throw new InvalidOperationException("URL do GitHub Models não configurada (GitHubModels:BaseUrl).");

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Itera360App");

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
