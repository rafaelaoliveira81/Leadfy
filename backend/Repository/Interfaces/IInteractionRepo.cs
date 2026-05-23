using Domain.Entities;

public interface IInteractionRepo
{
    Task<int> AddAsync(Interaction interaction);
    Task<Interaction> GetByIdAsync(int idInteraction);
    Task<IEnumerable<Interaction>> GetAllByOpportunityIdAsync(int opportunityId);
    Task<IEnumerable<Interaction>> GetLastInteractionsByOpportunityIdAsync(int opportunityId);
    Task UpdateAsync(Interaction interaction);
    Task DeleteAsync(Interaction interaction);
}