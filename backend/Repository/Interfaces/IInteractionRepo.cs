using Domain.Entities;

public interface IInteractionRepo  : IBaseRepository<Interaction>
{
    Task<IEnumerable<Interaction>> GetAllByOpportunityIdAsync(Guid opportunityId);
    Task<IEnumerable<Interaction>> GetLastInteractionsByOpportunityIdAsync(Guid opportunityId);
}