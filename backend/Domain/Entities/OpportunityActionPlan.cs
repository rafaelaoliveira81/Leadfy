namespace Domain.Entities;

public class OpportunityActionPlan
{
    public int Id { get; set; }
    public int OpportunityId { get; set; }
    public int AiConfigId { get; set; }
    public string ActionPlan { get; set; }
    public DateTime GeneratedAt { get; set; }

    // Relacionamentos
    public Opportunity Opportunity { get; set; }
    public AiConfig AiConfig { get; set; }

    public OpportunityActionPlan()
    {
        GeneratedAt = DateTime.UtcNow;
    }
}
