using Application.DTOs;

public interface IOpportunityActionPlanApp
{
    Task<OpportunityActionPlanDto> GenerateAsync(int opportunityId, int promptId);
    Task<IEnumerable<OpportunityActionPlanDto>> GetByOpportunityIdAsync(int opportunityId);
}