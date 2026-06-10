using Domain.Enuns;

namespace Domain.Entities;

public class Opportunity : IEntity
{
    public Guid Id { get; set; }
    public Guid LeadId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ProductId { get; set; }
    public decimal Amount { get; set; }
    public OpportunityStage Stage { get; set; }
    public int SortOrder { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
    public DateTime CreatedAt { get; set; }


    // Relacionamentos
    public Product Product { get; set; }
    public Lead Lead { get; set; }
    public User User { get; set; }
    public ICollection<OpportunityActionPlan> ActionPlans { get; set; }
    public ICollection<Interaction> Interactions { get; set; }

    public Opportunity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }
}