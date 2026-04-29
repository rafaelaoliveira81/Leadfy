namespace Domain.Entities;

/// <summary>
/// Configuração de IA para geração de planos de ação de conversão de leads.
/// </summary>
public class AiConfig
{
    public int Id { get; private set; }

    /// <summary>
    /// Template do prompt enviado à IA. Suporta os placeholders
    /// {{LeadName}}, {{LeadEmail}} e {{LeadPhone}}.
    /// </summary>
    public string PromptTemplate { get; private set; }

    /// <summary>
    /// Nome do modelo de IA a ser utilizado (ex: gpt-4.1, gpt-4o-mini, Meta-Llama-3.1-70B-Instruct).
    /// </summary>
    public string ModelName { get; private set; }

    /// <summary>
    /// Chave da API do GitHub Models armazenada de forma criptografada (AES-256).
    /// </summary>
    public string ApiKeyHash { get; private set; }

    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public AiConfig(string promptTemplate, string modelName, string apiKeyHash)
    {
        PromptTemplate = promptTemplate;
        ModelName = modelName;
        ApiKeyHash = apiKeyHash;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    private AiConfig() { }

    /// <summary>
    /// Atualiza o template, o modelo e opcionalmente a chave de API.
    /// Quando <paramref name="newApiKeyHash"/> for nulo ou vazio, a chave existente é mantida.
    /// </summary>
    public void Update(string promptTemplate, string modelName, string newApiKeyHash)
    {
        PromptTemplate = promptTemplate;
        ModelName = modelName;
        if (!string.IsNullOrWhiteSpace(newApiKeyHash))
            ApiKeyHash = newApiKeyHash;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
