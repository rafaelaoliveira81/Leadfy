namespace Api.Models.Ai.Request;

/// <summary>
/// Dados para envio de um prompt avulso ao modelo de IA.
/// </summary>
public class AiPromptRequest
{
    /// <summary>Mensagem a ser enviada ao modelo.</summary>
    public string Prompt { get; set; }

    /// <summary>ID da configuração de IA a ser utilizada (modelo e chave de API).</summary>
    public int ConfigId { get; set; }
}
