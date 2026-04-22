using Domain.Entities;

/// <summary>
/// Contrato da camada de aplicação para os casos de uso de interações.
/// </summary>
public interface IInteractionApp
{
    /// <summary>
    /// Adiciona uma interação ao histórico de uma opportunity.
    /// </summary>
    /// <param name="opportunityId">Identificador da opportunity.</param>
    /// <param name="interaction">Entidade da interação a ser registrada.</param>
    /// <returns>Identificador da interação criada.</returns>
    Task<int> AddToOpportunityAsync(int opportunityId, Interaction interaction);

    /// <summary>
    /// Obtém uma interação pelo seu identificador.
    /// </summary>
    /// <param name="idInteraction">Identificador da interação.</param>
    /// <returns>Interação encontrada.</returns>
    Task<Interaction> GetByIdAsync(int idInteraction);

    /// <summary>
    /// Obtém o histórico de interações de uma opportunity.
    /// </summary>
    /// <param name="opportunityId">Identificador da opportunity.</param>
    /// <returns>Coleção de interações associadas à opportunity.</returns>
    Task<IEnumerable<Interaction>> GetByOpportunityIdAsync(int opportunityId);

    /// <summary>
    /// Remove uma interação do sistema.
    /// </summary>
    /// <param name="idInteraction">Identificador da interação a ser removida.</param>
    Task DeleteAsync(int idInteraction);
}