using Application.DTO;

public interface IOpportunityApp
{
    Task<int> AddAsync(OpportunityAdd request, int idUser);
    Task<OpportunityResponse> GetByIdAsync(int idOpportunity);
    Task<IEnumerable<OpportunityResponse>> GetAllAsync();
    Task<IEnumerable<OpportunityResponse>> GetByStageAsync(int stage);
    Task UpdateAsync(int idOpportunity, OpportunityUpdate request);
    Task DeleteAsync(int idOpportunity);
    Task ChangeStageAsync(int idOpportunity, int newStage);
    Task UpdateSortOrderAsync(IEnumerable<OpportunitySortOrderItem> items);
}
