namespace Api.Models.AiConfig.Request;

/// <summary>
/// Dados para atualização de uma configuração de IA existente.
/// </summary>
public class AiConfigUpdate
{
    /// <summary>Novo template do prompt.</summary>
    public string PromptTemplate { get; set; }

    /// <summary>Novo nome do modelo de IA.</summary>
    public string ModelName { get; set; }

    /// <summary>
    /// Nova chave de API. Quando nula ou vazia, a chave existente é mantida.
    /// </summary>
    public string? ApiKey { get; set; }
}
