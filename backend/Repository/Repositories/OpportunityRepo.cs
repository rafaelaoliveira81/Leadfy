using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

/// <summary>
/// Repositório responsável pela persistência e consulta de opportunities.
/// Implementa operações CRUD utilizando Entity Framework Core.
/// </summary>
/// <remarks>
/// Esta classe pertence à camada de Repository e deve conter apenas
/// lógica de acesso a dados, sem regras de negócio.
/// </remarks>
public class OpportunityRepo : BaseRepo, IOpportunityRepo
{
    public OpportunityRepo(CRMContext context) : base(context)
    {
    }

    /// <summary>
    /// Adiciona uma nova opportunity no banco de dados.
    /// </summary>
    /// <param name="opportunity">Entidade da opportunity a ser persistida.</param>
    /// <returns>Retorna o ID da opportunity gerado após a inserção.</returns>
    public async Task<int> AddAsync(Opportunity opportunity)
    {
        _context.Opportunities.Add(opportunity);
        await _context.SaveChangesAsync();

        return opportunity.ID;
    }

    /// <summary>
    /// Busca uma opportunity pelo seu identificador único.
    /// </summary>
    /// <param name="idOpportunity">ID da opportunity.</param>
    /// <returns>Opportunity encontrada ou null caso não exista.</returns>
    public async Task<Opportunity> GetByIdAsync(int idOpportunity)
    {
        return await _context.Opportunities
            .Include(o => o.Lead)
            .Include(o => o.Owner)
            .Include(o => o.Product)
            .FirstOrDefaultAsync(o => o.ID == idOpportunity);
    }

    /// <summary>
    /// Busca opportunities cujo título contenha o valor informado.
    /// </summary>
    /// <param name="titleOpportunity">
    /// Texto utilizado para filtrar as opportunities pelo título.
    /// A busca é case-insensitive.
    /// </param>
    /// <returns>
    /// Uma coleção de opportunities que possuem o título contendo o valor informado.
    /// Retorna uma lista vazia caso nenhuma opportunity seja encontrada.
    /// </returns>
    public async Task<IEnumerable<Opportunity>> GetByTitleContainingAsync(string titleOpportunity)
    {
        return await _context.Opportunities
            .Where(o => EF.Functions.Like(o.Title, $"%{titleOpportunity}%"))
            .Include(o => o.Lead)
            .Include(o => o.Owner)
            .Include(o => o.Product)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna todas as opportunities cadastradas.
    /// </summary>
    /// <returns>Lista de opportunities.</returns>
    public async Task<IEnumerable<Opportunity>> GetAllAsync()
    {
        return await _context.Opportunities
            .Include(o => o.Lead)
            .Include(o => o.Owner)
            .Include(o => o.Product)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna todas as opportunities filtrando pelo status (ativo/inativo).
    /// </summary>
    /// <param name="statusOpportunity">Status da opportunity (true = ativo, false = inativo).</param>
    /// <returns>Lista de opportunities filtradas.</returns>
    public async Task<IEnumerable<Opportunity>> GetAllByStatusAsync(bool statusOpportunity)
    {
        return await _context.Opportunities
            .Where(o => o.IsActive == statusOpportunity)
            .Include(o => o.Lead)
            .Include(o => o.Owner)
            .Include(o => o.Product)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna todas as opportunities associadas a um lead específico.
    /// </summary>
    /// <param name="leadId">ID do lead.</param>
    /// <returns>Lista de opportunities do lead.</returns>
    public async Task<IEnumerable<Opportunity>> GetByLeadIdAsync(int leadId)
    {
        return await _context.Opportunities
            .Where(o => o.LeadId == leadId)
            .Include(o => o.Lead)
            .Include(o => o.Owner)
            .Include(o => o.Product)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna todas as opportunities associadas a um owner específico.
    /// </summary>
    /// <param name="ownerId">ID do owner.</param>
    /// <returns>Lista de opportunities do owner.</returns>
    public async Task<IEnumerable<Opportunity>> GetByOwnerIdAsync(int ownerId)
    {
        return await _context.Opportunities
            .Where(o => o.OwnerId == ownerId)
            .Include(o => o.Lead)
            .Include(o => o.Owner)
            .Include(o => o.Product)
            .ToListAsync();
    }

    /// <summary>
    /// Atualiza os dados de uma opportunity existente.
    /// </summary>
    /// <param name="opportunity">Opportunity com dados atualizados.</param>
    /// <remarks>
    /// O Entity Framework irá rastrear as alterações e persistir no banco.
    /// </remarks>
    public async Task UpdateAsync(Opportunity opportunity)
    {
        _context.Opportunities.Update(opportunity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove uma opportunity do banco de dados.
    /// </summary>
    /// <param name="opportunity">Opportunity a ser removida.</param>
    /// <remarks>
    /// Este método realiza remoção física do registro.
    /// </remarks>
    public async Task DeleteAsync(Opportunity opportunity)
    {
        _context.Opportunities.Remove(opportunity);
        await _context.SaveChangesAsync();
    }
}
