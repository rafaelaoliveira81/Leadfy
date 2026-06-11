namespace Domain.Entities;

public class Interaction : IEntity
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid OpportunityId { get; set; }
    public Guid UserId { get; set; }
    public int? FromStage { get; set; }
    public int? ToStage { get; set; }
    public string Description { get; set; }
    public DateTime InteractionDate { get; set; }
    public DateTime? NextContactDate { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relacionamentos
    public Tenant Tenant { get; set; }
    public Opportunity Opportunity { get; set; }
    public User User { get; set; }

    public Interaction()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        InteractionDate = DateTime.Now;
    }
}