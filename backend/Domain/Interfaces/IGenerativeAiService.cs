namespace Domain.Interfaces;

/// <summary>
/// Contrato para geração de conteúdo via provedor de IA.
/// </summary>
public interface IGenerativeAiService
{
    /// <summary>
    /// Envia um prompt ao modelo informado usando a chave de API fornecida.
    /// </summary>
    /// <param name="prompt">Prompt final a ser enviado ao modelo.</param>
    /// <param name="modelName">Nome do modelo configurado.</param>
    /// <param name="apiKey">Chave da API em texto plano.</param>
    /// <returns>Conteúdo gerado pelo modelo.</returns>
    Task<string> GenerateAsync(string prompt, string modelName, string apiKey);
}