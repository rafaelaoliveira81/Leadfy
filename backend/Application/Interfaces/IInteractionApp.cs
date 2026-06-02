using Application.DTO;

public interface IInteractionApp
{
    Task<int> AddToOpportunityAsync(int opportunityId, InteractionAdd interaction);
    Task<InteractionResponse> GetByIdAsync(int idInteraction);
    Task<IEnumerable<InteractionResponse>> GetByOpportunityIdAsync(int opportunityId);
    Task DeleteAsync(int idInteraction);
}