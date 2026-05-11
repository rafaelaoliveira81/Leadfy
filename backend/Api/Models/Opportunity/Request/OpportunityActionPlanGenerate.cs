namespace Models.Request;

/// <summary>
/// Dados necessários para gerar um plano de ação de uma opportunity.
/// </summary>
public class OpportunityActionPlanGenerate
{
    /// <summary>
    /// Identificador da configuração de IA a ser utilizada.
    /// </summary>
    public int ConfigId { get; set; }
}