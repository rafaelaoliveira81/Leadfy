namespace Application.DTO;

public class OpportunityActionPlanDto
{
    public string Id { get; set; }
    public string OpportunityId { get; set; }
    public string ActionPlan { get; set; }
    public string Message { get; set; }
    public DateTime GeneratedAt { get; set; }
}
