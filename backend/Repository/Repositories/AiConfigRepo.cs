using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

/// <summary>
/// Repositório responsável pela persistência e consulta de configurações de IA.
/// </summary>
public class AiConfigRepo : BaseRepo, IAiConfigRepo
{
    public AiConfigRepo(CRMContext context) : base(context) { }

    /// <summary>
    /// Adiciona uma nova configuração de IA no banco de dados.
    /// </summary>
    /// <param name="config">Entidade a ser persistida.</param>
    /// <returns>ID gerado após a inserção.</returns>
    public async Task<int> AddAsync(AiConfig config)
    {
        _context.AiConfigs.Add(config);
        await _context.SaveChangesAsync();
        return config.Id;
    }

    /// <summary>
    /// Busca uma configuração pelo seu identificador único.
    /// </summary>
    /// <param name="id">ID da configuração.</param>
    /// <returns>Configuração encontrada ou null.</returns>
    public async Task<AiConfig> GetByIdAsync(int id)
    {
        return await _context.AiConfigs
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Retorna a configuração ativa mais recentemente criada.
    /// </summary>
    /// <returns>Configuração ativa ou null se nenhuma existir.</returns>
    public async Task<AiConfig> GetActiveAsync()
    {
        return await _context.AiConfigs
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retorna todas as configurações cadastradas, ordenadas da mais recente para a mais antiga.
    /// </summary>
    public async Task<IEnumerable<AiConfig>> GetAllAsync()
    {
        return await _context.AiConfigs
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Atualiza os dados de uma configuração existente.
    /// </summary>
    public async Task UpdateAsync(AiConfig config)
    {
        _context.AiConfigs.Update(config);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove uma configuração do banco de dados.
    /// </summary>
    public async Task DeleteAsync(AiConfig config)
    {
        _context.AiConfigs.Remove(config);
        await _context.SaveChangesAsync();
    }
}
