using Domain.Entities;
public interface IOpportunityActionPlanApp
{
    Task<Opportunity> GenerateAsync(int opportunityId, int configId);
}