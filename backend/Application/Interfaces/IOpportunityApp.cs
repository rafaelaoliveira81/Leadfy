using Application.DTO;

public interface IOpportunityApp
{
    Task<string> AddAsync(OpportunityAdd request, string idUser);
    Task<OpportunityResponse> GetByIdAsync(string idOpportunity);
    Task<IEnumerable<OpportunityResponse>> GetAllAsync();
    Task<IEnumerable<OpportunityResponse>> GetByStageAsync(int stage);
    Task UpdateAsync(string idOpportunity, OpportunityUpdate request);
    Task DeleteAsync(string idOpportunity);
    Task ChangeStageAsync(string idOpportunity, int newStage);
    Task UpdateSortOrderAsync(IEnumerable<OpportunitySortOrderItem> items);
}
