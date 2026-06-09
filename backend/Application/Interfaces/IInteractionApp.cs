using Application.DTO;

public interface IInteractionApp
{
    Task<int> AddToOpportunityAsync(int opportunityId, InteractionRequest interaction);
    Task<InteractionResponse> GetByIdAsync(int idInteraction);
    Task<IEnumerable<InteractionResponse>> GetByOpportunityIdAsync(int opportunityId);
    Task DeleteAsync(int idInteraction);
    Task<IteractionOptimizeDTO> OptimizeInteractionAsync(IteractionOptimizeDTO interaction);
}