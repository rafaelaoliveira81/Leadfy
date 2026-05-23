using Domain.Entities;
using Domain.Enuns;

public interface IOpportunityApp
{
    Task<int> AddAsync(Opportunity opportunity);
    Task<Opportunity> GetByIdAsync(int idOpportunity);
    Task<IEnumerable<Opportunity>> GetAllAsync();
    Task<IEnumerable<Opportunity>> GetAllByStatusAsync(bool statusOpportunity);
    Task<IEnumerable<Opportunity>> GetByLeadIdAsync(int leadId);
    Task UpdateAsync(Opportunity opportunity);
    Task DeleteAsync(int idOpportunity);
    Task DeactivateAsync(int idOpportunity);
    Task ActivateAsync(int idOpportunity);
    Task ChangeStageAsync(int idOpportunity, OpportunityStage newStage);
    Task UpdateSortOrderAsync(IEnumerable<(int id, OpportunityStage stage, int sortOrder)> items);
}
