using Domain.Enuns;

namespace Domain.Entities;

public class Opportunity
{
    public int ID { get; set; }
    public int LeadId { get; set; }
    public int UserId { get; set; }
    public int? ProductId { get; set; }
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
        CreatedAt = DateTime.Now;
    }
}