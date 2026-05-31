using Domain.Entities;

public interface IOpportunityRepo
{
    Task<int> AddAsync(Opportunity opportunity);
    Task<Opportunity> GetByIdAsync(int idOpportunity);
    Task<IEnumerable<Opportunity>> GetAllAsync();
    Task<IEnumerable<Opportunity>> GetByStageAsync(int stage);
    Task UpdateAsync(Opportunity opportunity);
    Task DeleteAsync(Opportunity opportunity);
}
