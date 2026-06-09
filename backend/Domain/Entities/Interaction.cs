namespace Domain.Entities;

public class Interaction
{
    public int Id { get; set; }
    public int OpportunityId { get; set; }
    public Guid UserId { get; set; }
    public int? FromStage { get; set; }
    public int? ToStage { get; set; }
    public string Description { get; set; }
    public DateTime InteractionDate { get; set; }
    public DateTime? NextContactDate { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relacionamentos
    public Opportunity Opportunity { get; set; }
    public User User { get; set; }

    public Interaction()
    {
        CreatedAt = DateTime.Now;
        InteractionDate = DateTime.Now;
    }
}