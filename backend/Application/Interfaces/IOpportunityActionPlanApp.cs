using Application.DTO;

public interface IOpportunityActionPlanApp
{
    Task<OpportunityActionPlanDto> GenerateAsync(int opportunityId, string promptId);
    Task<IEnumerable<OpportunityActionPlanDto>> GetByOpportunityIdAsync(int opportunityId);
}