using Domain.Entities;

public interface IOpportunityRepo  : IBaseRepository<Opportunity>
{
    Task<IEnumerable<Opportunity>> GetByStageAsync(Guid tenantId, int stage);
}
