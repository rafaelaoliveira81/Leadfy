using Domain.Entities;
using Domain.Enuns;

/// <summary>
/// Contrato de persistência para operações de leitura e escrita de interações.
/// </summary>
public interface IInteractionRepo
{
    /// <summary>
    /// Adiciona uma nova interação ao banco de dados.
    /// </summary>
    /// <param name="interaction">Entidade de interação a ser persistida.</param>
    /// <returns>Identificador gerado para a interação.</returns>
    Task<int> AddAsync(Interaction interaction);

    /// <summary>
    /// Busca uma interação pelo seu identificador único.
    /// </summary>
    /// <param name="idInteraction">Identificador da interação.</param>
    /// <returns>Interação encontrada ou null caso não exista.</returns>
    Task<Interaction> GetByIdAsync(int idInteraction);

    /// <summary>
    /// Obtém as interações vinculadas a uma entidade do CRM.
    /// </summary>
    /// <param name="crmEntityType">Tipo da entidade do CRM.</param>
    /// <param name="crmEntityId">Identificador da entidade do CRM.</param>
    /// <returns>Coleção de interações encontradas para a entidade informada.</returns>
    Task<IEnumerable<Interaction>> GetByCrmEntityAsync(CrmEntityType crmEntityType, int crmEntityId);

    /// <summary>
    /// Obtém as interações associadas a uma opportunity.
    /// </summary>
    /// <param name="opportunityId">Identificador da opportunity.</param>
    /// <returns>Coleção de interações da opportunity.</returns>
    Task<IEnumerable<Interaction>> GetByOpportunityIdAsync(int opportunityId);

    /// <summary>
    /// Atualiza uma interação existente.
    /// </summary>
    /// <param name="interaction">Entidade com os dados atualizados.</param>
    Task UpdateAsync(Interaction interaction);

    /// <summary>
    /// Remove uma interação do banco de dados.
    /// </summary>
    /// <param name="interaction">Entidade a ser removida.</param>
    Task DeleteAsync(Interaction interaction);
}