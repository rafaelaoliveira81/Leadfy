using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

/// <summary>
/// Repositório responsável pela persistência e consulta de leads.
/// Implementa operações CRUD utilizando Entity Framework Core.
/// </summary>
/// <remarks>
/// Esta classe pertence à camada de Repository e deve conter apenas
/// lógica de acesso a dados, sem regras de negócio.
/// </remarks>
public class LeadRepo : BaseRepo, ILeadRepo
{
    public LeadRepo(CRMContext context) : base(context)
    {
    }

    /// <summary>
    /// Adiciona um novo lead no banco de dados.
    /// </summary>
    /// <param name="lead">Entidade do lead a ser persistida.</param>
    /// <returns>Retorna o ID do lead gerado após a inserção.</returns>
    public async Task<int> AddAsync(Lead lead)
    {
        _context.Leads.Add(lead);
        await _context.SaveChangesAsync();

        return lead.Id;
    }

    /// <summary>
    /// Busca um lead pelo seu identificador único.
    /// </summary>
    /// <param name="idLead">ID do lead.</param>
    /// <returns>Lead encontrado ou null caso não exista.</returns>
    public async Task<Lead> GetByIdAsync(int idLead)
    {
        return await _context.Leads
            .FirstOrDefaultAsync(l => l.Id == idLead);
    }

    /// <summary>
    /// Busca leads cujo nome contenha o valor informado.
    /// </summary>
    /// <param name="nameLead">
    /// Texto utilizado para filtrar os leads pelo nome.
    /// A busca é case-insensitive.
    /// </param>
    /// <returns>
    /// Uma coleção de leads que possuem o nome contendo o valor informado.
    /// Retorna uma lista vazia caso nenhum lead seja encontrado.
    /// </returns>
    public async Task<IEnumerable<Lead>> GetByNameContainingAsync(string nameLead)
    {
        return await _context.Leads
            .Where(l => EF.Functions.Like(l.Name, $"%{nameLead}%"))
            .ToListAsync();
    }

    /// <summary>
    /// Retorna todos os leads cadastrados.
    /// </summary>
    /// <returns>Lista de leads.</returns>
    public async Task<IEnumerable<Lead>> GetAllAsync()
    {
        return await _context.Leads.ToListAsync();
    }

    /// <summary>
    /// Retorna todos os leads filtrando pelo status (ativo/inativo).
    /// </summary>
    /// <param name="statusLead">Status do lead (true = ativo, false = inativo).</param>
    /// <returns>Lista de leads filtrados.</returns>
    public async Task<IEnumerable<Lead>> GetAllByStatusAsync(bool statusLead)
    {
        return await _context.Leads
            .Where(l => l.IsActive == statusLead)
            .ToListAsync();
    }

    /// <summary>
    /// Atualiza os dados de um lead existente.
    /// </summary>
    /// <param name="lead">Lead com dados atualizados.</param>
    /// <remarks>
    /// O Entity Framework irá rastrear as alterações e persistir no banco.
    /// </remarks>
    public async Task UpdateAsync(Lead lead)
    {
        _context.Leads.Update(lead);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove um lead do banco de dados.
    /// </summary>
    /// <param name="lead">Lead a ser removido.</param>
    public async Task DeleteAsync(Lead lead)
    {
        _context.Leads.Remove(lead);
        await _context.SaveChangesAsync();
    }
}
