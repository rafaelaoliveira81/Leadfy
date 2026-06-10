using Domain.Entities;

public interface IOpportunityActionPlanRepo  : IBaseRepository<OpportunityActionPlan>
{
    Task<IEnumerable<OpportunityActionPlan>> GetByOpportunityIdAsync(Guid opportunityId);
}
