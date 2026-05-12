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

    public async Task<int> AddAsync(AiConfig config)
    {
        _context.AiConfigs.Add(config);
        await _context.SaveChangesAsync();
        return config.Id;
    }

    public async Task<AiConfig> GetByIdAsync(int id)
    {
        return await _context.AiConfigs
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<AiConfig> GetActiveAsync()
    {
        return await _context.AiConfigs
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<AiConfig>> GetAllAsync()
    {
        return await _context.AiConfigs
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
    public async Task UpdateAsync(AiConfig config)
    {
        _context.AiConfigs.Update(config);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(AiConfig config)
    {
        _context.AiConfigs.Remove(config);
        await _context.SaveChangesAsync();
    }
}
