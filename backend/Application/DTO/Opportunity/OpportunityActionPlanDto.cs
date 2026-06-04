namespace Application.DTO;

public class OpportunityActionPlanDto
{
    public int Id { get; set; }
    public int OpportunityId { get; set; }
    public string ActionPlan { get; set; }
    public string Message { get; set; }
    public DateTime GeneratedAt { get; set; }
}
