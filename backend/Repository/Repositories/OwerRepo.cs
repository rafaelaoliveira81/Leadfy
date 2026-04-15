using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

/// <summary>
/// Repositório responsável pela persistência e consulta de owers.
/// Implementa operações CRUD utilizando Entity Framework Core.
/// </summary>
/// <remarks>
/// Esta classe pertence à camada de Repository e deve conter apenas
/// lógica de acesso a dados, sem regras de negócio.
/// </remarks>
public class OwerRepo : BaseRepo, IOwerRepo
{
    public OwerRepo(CRMContext context) : base(context)
    {
    }

    /// <summary>
    /// Adiciona um novo ower no banco de dados.
    /// </summary>
    /// <param name="ower">Entidade do ower a ser persistida.</param>
    /// <returns>Retorna o ID do ower gerado após a inserção.</returns>
    public async Task<int> AddAsync(Ower ower)
    {
        _context.Owers.Add(ower);
        await _context.SaveChangesAsync();

        return ower.ID;
    }

    /// <summary>
    /// Busca um ower pelo seu identificador único.
    /// </summary>
    /// <param name="idOwer">ID do ower.</param>
    /// <returns>Ower encontrado ou null caso não exista.</returns>
    public async Task<Ower> GetByIdAsync(int idOwer)
    {
        return await _context.Owers
            .Include(u => u.User)
            .FirstOrDefaultAsync(o => o.ID == idOwer);
    }

    /// <summary>
    /// Busca owers cujo nome contenha o valor informado.
    /// </summary>
    /// <param name="nameOwer">
    /// Texto utilizado para filtrar os owers pelo nome.
    /// A busca é case-insensitive.
    /// </param>
    /// <returns>
    /// Uma coleção de owers que possuem o nome contendo o valor informado.
    /// Retorna uma lista vazia caso nenhum ower seja encontrado.
    /// </returns>
    public async Task<IEnumerable<Ower>> GetByNameContainingAsync(string nameOwer)
    {
        return await _context.Owers
            .Where(o => EF.Functions.Like(o.Name, $"%{nameOwer}%"))
            .Include(u => u.User)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna todos os owers cadastrados.
    /// </summary>
    /// <returns>Lista de owers.</returns>
    public async Task<IEnumerable<Ower>> GetAllAsync()
    {
        return await _context.Owers
            .Include(u => u.User)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna todos os owers filtrando pelo status (ativo/inativo).
    /// </summary>
    /// <param name="statusOwer">Status do ower (true = ativo, false = inativo).</param>
    /// <returns>Lista de owers filtrados.</returns>
    public async Task<IEnumerable<Ower>> GetAllByStatusAsync(bool statusOwer)
    {
        return await _context.Owers
            .Where(o => o.IsActive == statusOwer)
            .Include(u => u.User)
            .ToListAsync();
    }

    /// <summary>
    /// Atualiza os dados de um ower existente.
    /// </summary>
    /// <param name="ower">Ower com dados atualizados.</param>
    /// <remarks>
    /// O Entity Framework irá rastrear as alterações e persistir no banco.
    /// </remarks>
    public async Task UpdateAsync(Ower ower)
    {
        _context.Owers.Update(ower);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove um ower do banco de dados.
    /// </summary>
    /// <param name="ower">Ower a ser removido.</param>
    public async Task DeleteAsync(Ower ower)
    {
        _context.Owers.Remove(ower);
        await _context.SaveChangesAsync();
    }
}