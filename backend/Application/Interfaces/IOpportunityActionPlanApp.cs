using Application.DTO;

public interface IOpportunityActionPlanApp
{
    Task<OpportunityActionPlanDto> GenerateAsync(int opportunityId, int promptId);
    Task<IEnumerable<OpportunityActionPlanDto>> GetByOpportunityIdAsync(int opportunityId);
}