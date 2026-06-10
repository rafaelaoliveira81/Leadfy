namespace Domain.Entities;

public class OpportunityActionPlan : IEntity
{
    public Guid Id { get; set; }
    public Guid OpportunityId { get; set; }
    public string ActionPlan { get; set; }
    public string Message { get; set; }
    public DateTime GeneratedAt { get; set; }

    // Relacionamentos
    public Opportunity Opportunity { get; set; }

    public OpportunityActionPlan()
    {
        Id = Guid.NewGuid();
        GeneratedAt = DateTime.Now;
    }
}
