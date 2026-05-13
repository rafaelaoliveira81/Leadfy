using Domain.Entities;

/// <summary>
/// Contrato do caso de uso responsável por gerar plano de ação para uma opportunity.
/// </summary>
public interface IOpportunityActionPlanApp
{
    /// <summary>
    /// Gera e persiste um plano de ação para a opportunity informada usando a configuração de IA escolhida.
    /// </summary>
    /// <param name="opportunityId">Identificador da opportunity.</param>
    /// <param name="configId">Identificador da configuração de IA.</param>
    /// <returns>Opportunity atualizada com o plano de ação persistido.</returns>
    Task<Opportunity> GenerateAsync(int opportunityId, int configId);
}