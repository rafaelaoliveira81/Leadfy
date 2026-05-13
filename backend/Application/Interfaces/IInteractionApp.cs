using Domain.Entities;
public interface IInteractionApp
{
    Task<int> AddToOpportunityAsync(int opportunityId, Interaction interaction);
    Task<Interaction> GetByIdAsync(int idInteraction);
    Task<IEnumerable<Interaction>> GetByOpportunityIdAsync(int opportunityId);
    Task DeleteAsync(int idInteraction);
}