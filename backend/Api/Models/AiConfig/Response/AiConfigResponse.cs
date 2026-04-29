namespace Api.Models.AiConfig.Response;

/// <summary>
/// Dados de uma configuração de IA retornados pela API.
/// A chave de API é mascarada — apenas os últimos 4 caracteres são exibidos.
/// </summary>
public class AiConfigResponse
{
    public int Id { get; set; }
    public string PromptTemplate { get; set; }
    public string ModelName { get; set; }

    /// <summary>
    /// Chave de API mascarada (ex: ****3MupRh). A chave completa nunca é exposta pela API.
    /// </summary>
    public string ApiKeyMasked { get; set; }

    public bool IsActive { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
