using Domain.Entities;

public interface IOpportunityActionPlanRepo
{
    Task<int> AddAsync(OpportunityActionPlan actionPlan);
    Task<OpportunityActionPlan> GetByIdAsync(int id);
    Task<IEnumerable<OpportunityActionPlan>> GetByOpportunityIdAsync(int opportunityId);
}
