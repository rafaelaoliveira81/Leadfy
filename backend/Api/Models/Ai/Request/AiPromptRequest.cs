namespace Api.Models.Ai.Request;

/// <summary>
/// Dados para envio de um prompt avulso ao modelo de IA.
/// </summary>
public class AiPromptRequest
{
    /// <summary>Mensagem a ser enviada ao modelo.</summary>
    public string Prompt { get; set; }

    /// <summary>Nome do modelo (ex: gpt-4.1, gpt-4o-mini).</summary>
    public string ModelName { get; set; }

    /// <summary>Chave da API do GitHub Models.</summary>
    public string ApiKey { get; set; }
}
