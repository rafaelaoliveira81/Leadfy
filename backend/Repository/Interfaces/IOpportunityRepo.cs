using Domain.Entities;

public interface IOpportunityRepo  : IBaseRepository<Opportunity>
{
    Task<IEnumerable<Opportunity>> GetByStageAsync(int stage);
}
