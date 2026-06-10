using Application.DTO;

public interface IOpportunityActionPlanApp
{
    Task<OpportunityActionPlanDto> GenerateAsync(string opportunityId, string promptId);
    Task<IEnumerable<OpportunityActionPlanDto>> GetByOpportunityIdAsync(string opportunityId);
}