using Application.DTOs;

public interface IOpportunityActionPlanApp
{
    Task<OpportunityActionPlanDto> GenerateAsync(int opportunityId, int configId);
    Task<IEnumerable<OpportunityActionPlanDto>> GetByOpportunityIdAsync(int opportunityId);
}