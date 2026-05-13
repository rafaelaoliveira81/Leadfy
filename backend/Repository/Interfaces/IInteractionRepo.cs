using Domain.Entities;
using Domain.Enuns;
public interface IInteractionRepo
{
    Task<int> AddAsync(Interaction interaction);
    Task<Interaction> GetByIdAsync(int idInteraction);
    Task<IEnumerable<Interaction>> GetByCrmEntityAsync(CrmEntityType crmEntityType, int crmEntityId);
    Task<IEnumerable<Interaction>> GetByOpportunityIdAsync(int opportunityId);
    Task UpdateAsync(Interaction interaction);
    Task DeleteAsync(Interaction interaction);
}