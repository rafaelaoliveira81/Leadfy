using Application.DTO;
using Domain.Entities;

namespace Application;

public interface IAiConfigApp
{
    Task<int> AddAsync(AiConfigRequest request);
    Task<AiConfig> GetByIdAsync(int id);
    Task<AiConfig?> GetActiveAsync();
    Task<IEnumerable<AiConfig>> GetAllAsync();
    Task UpdateAsync(int id, AiConfigRequest request);
    Task DeleteAsync(int id);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    string DecryptApiKey(string encryptedApiKey);
    string BuildPrompt(AiConfig config, Lead lead);
}
