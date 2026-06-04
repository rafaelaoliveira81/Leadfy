namespace Domain.Entities;

public class OpportunityActionPlan
{
    public int Id { get; set; }
    public int OpportunityId { get; set; }
    public string ActionPlan { get; set; }
    public string Message { get; set; }
    public DateTime GeneratedAt { get; set; }

    // Relacionamentos
    public Opportunity Opportunity { get; set; }

    public OpportunityActionPlan()
    {
        GeneratedAt = DateTime.Now;
    }
}
