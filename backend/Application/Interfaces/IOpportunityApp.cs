using Domain.Entities;

public interface IOpportunityApp
{
    Task<int> AddAsync(Opportunity opportunity);
    Task<Opportunity> GetByIdAsync(int idOpportunity);
    Task<IEnumerable<Opportunity>> GetByTitleContainingAsync(string titleOpportunity);
    Task<IEnumerable<Opportunity>> GetAllAsync();
    Task<IEnumerable<Opportunity>> GetAllByStatusAsync(bool statusOpportunity);
    Task<IEnumerable<Opportunity>> GetByLeadIdAsync(int leadId);
    Task<IEnumerable<Opportunity>> GetByOwnerIdAsync(int ownerId);
    Task UpdateAsync(Opportunity opportunity);
    Task DeleteAsync(int idOpportunity);
    Task DeactivateAsync(int idOpportunity);
    Task ActivateAsync(int idOpportunity);
}
