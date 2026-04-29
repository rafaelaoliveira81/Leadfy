namespace Api.Models.AiConfig.Request;

/// <summary>
/// Dados necessários para criar uma nova configuração de IA.
/// </summary>
public class AiConfigSave
{
    /// <summary>
    /// Template do prompt enviado ao modelo. Suporta os placeholders
    /// {{LeadName}}, {{LeadEmail}} e {{LeadPhone}}.
    /// </summary>
    public string PromptTemplate { get; set; }

    /// <summary>
    /// Nome do modelo de IA (ex: gpt-4.1, gpt-4o-mini, Meta-Llama-3.1-70B-Instruct).
    /// </summary>
    public string ModelName { get; set; }

    /// <summary>
    /// Chave pessoal de acesso à API do GitHub Models (GitHub Personal Access Token).
    /// Será armazenada de forma criptografada.
    /// </summary>
    public string ApiKey { get; set; }
}
