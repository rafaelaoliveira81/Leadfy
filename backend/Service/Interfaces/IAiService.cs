/// <summary>
/// Contrato para chamadas à API de modelos de IA (GitHub Models).
/// </summary>
public interface IAiService
{
    /// <summary>
    /// Envia um prompt ao modelo de IA especificado usando a chave de API fornecida.
    /// </summary>
    /// <param name="prompt">Mensagem a ser enviada ao modelo.</param>
    /// <param name="modelName">Nome do modelo (ex: gpt-4.1, gpt-4o-mini).</param>
    /// <param name="apiKey">Chave da API do GitHub Models em texto plano.</param>
    /// <returns>Resposta gerada pelo modelo, ou null em caso de resposta vazia.</returns>
    Task<string?> GetResponseFromModel(string prompt, string modelName, string apiKey);
}
