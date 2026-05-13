namespace Models.Response;

/// <summary>
/// Modelo de resposta para geração do plano de ação de uma opportunity.
/// </summary>
public class OpportunityActionPlanResponse
{
    /// <summary>
    /// Identificador da opportunity.
    /// </summary>
    public int OpportunityId { get; set; }

    /// <summary>
    /// Identificador da configuração de IA utilizada.
    /// </summary>
    public int ConfigId { get; set; }

    /// <summary>
    /// Título da opportunity.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Plano de ação gerado e persistido.
    /// </summary>
    public string ActionPlan { get; set; }

    /// <summary>
    /// Data e hora da geração do plano.
    /// </summary>
    public DateTime? ActionPlanGeneratedAt { get; set; }
}