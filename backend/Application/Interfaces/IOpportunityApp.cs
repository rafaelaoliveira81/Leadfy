using Application.DTO;

public interface IOpportunityApp
{
    Task<int> AddAsync(OpportunityAdd request);
    Task<OpportunityResponse> GetByIdAsync(int idOpportunity);
    Task<IEnumerable<OpportunityResponse>> GetAllAsync();
    Task<IEnumerable<OpportunityResponse>> GetAllByStatusAsync(bool statusOpportunity);
    Task<IEnumerable<OpportunityResponse>> GetByStageAsync(int stage);
    Task<IEnumerable<OpportunityResponse>> GetByLeadIdAsync(int leadId);
    Task UpdateAsync(int idOpportunity, OpportunityUpdate request);
    Task DeleteAsync(int idOpportunity);
    Task DeactivateAsync(int idOpportunity);
    Task ActivateAsync(int idOpportunity);
    Task ChangeStageAsync(int idOpportunity, int newStage);
    Task UpdateSortOrderAsync(IEnumerable<OpportunitySortOrderItem> items);
}
