using Application.DTO;

public interface IInteractionApp
{
    Task<string> AddToOpportunityAsync(InteractionRequest interaction);
    Task<InteractionResponse> GetByIdAsync(string idInteraction);
    Task<IEnumerable<InteractionResponse>> GetByOpportunityIdAsync(string opportunityId);
    Task DeleteAsync(string idInteraction);
    Task<IteractionOptimizeDTO> OptimizeInteractionAsync(IteractionOptimizeDTO interaction);
}