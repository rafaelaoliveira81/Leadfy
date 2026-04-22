using Domain.Entities;
using Domain.Enuns;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

namespace Repository.Repositories;

/// <summary>
/// Repositório responsável pela persistência e consulta de interações.
/// Implementa operações de leitura e escrita utilizando Entity Framework Core.
/// </summary>
/// <remarks>
/// Esta classe pertence à camada de Repository e deve conter apenas
/// lógica de acesso a dados, sem regras de negócio.
/// </remarks>
public class InteractionRepo : BaseRepo, IInteractionRepo
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="InteractionRepo"/>.
    /// </summary>
    /// <param name="context">Contexto do Entity Framework Core.</param>
    public InteractionRepo(CRMContext context) : base(context)
    {
    }

    /// <summary>
    /// Adiciona uma nova interação no banco de dados.
    /// </summary>
    /// <param name="interaction">Entidade da interação a ser persistida.</param>
    /// <returns>Retorna o ID da interação gerado após a inserção.</returns>
    public async Task<int> AddAsync(Interaction interaction)
    {
        _context.Interactions.Add(interaction);
        await _context.SaveChangesAsync();

        return interaction.Id;
    }

    /// <summary>
    /// Busca uma interação pelo seu identificador único.
    /// </summary>
    /// <param name="idInteraction">ID da interação.</param>
    /// <returns>Interação encontrada ou null caso não exista.</returns>
    public async Task<Interaction> GetByIdAsync(int idInteraction)
    {
        return await _context.Interactions
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.Id == idInteraction);
    }

    /// <summary>
    /// Retorna as interações de uma entidade do CRM, ordenadas da mais recente para a mais antiga.
    /// </summary>
    /// <param name="crmEntityType">Tipo da entidade do CRM.</param>
    /// <param name="crmEntityId">Identificador da entidade do CRM.</param>
    /// <returns>Lista de interações vinculadas à entidade informada.</returns>
    public async Task<IEnumerable<Interaction>> GetByCrmEntityAsync(CrmEntityType crmEntityType, int crmEntityId)
    {
        return await _context.Interactions
            .Where(i => i.CrmEntityType == crmEntityType && i.CrmEntityId == crmEntityId)
            .Include(i => i.User)
            .OrderByDescending(i => i.InteractionDate)
            .ThenByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna as interações vinculadas a uma opportunity específica.
    /// </summary>
    /// <param name="opportunityId">ID da opportunity.</param>
    /// <returns>Lista de interações da opportunity.</returns>
    public async Task<IEnumerable<Interaction>> GetByOpportunityIdAsync(int opportunityId)
    {
        return await GetByCrmEntityAsync(CrmEntityType.Opportunity, opportunityId);
    }

    /// <summary>
    /// Atualiza os dados de uma interação existente.
    /// </summary>
    /// <param name="interaction">Interação com os dados atualizados.</param>
    public async Task UpdateAsync(Interaction interaction)
    {
        _context.Interactions.Update(interaction);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove uma interação do banco de dados.
    /// </summary>
    /// <param name="interaction">Interação a ser removida.</param>
    public async Task DeleteAsync(Interaction interaction)
    {
        _context.Interactions.Remove(interaction);
        await _context.SaveChangesAsync();
    }
}