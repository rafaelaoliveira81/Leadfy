using Domain.Entities;
public interface IAiConfigRepo
{
    Task<int> AddAsync(AiConfig config);
    Task<AiConfig> GetByIdAsync(int id);
    Task<AiConfig> GetActiveAsync();
    Task<IEnumerable<AiConfig>> GetAllAsync();
    Task UpdateAsync(AiConfig config);
    Task DeleteAsync(AiConfig config);
}
