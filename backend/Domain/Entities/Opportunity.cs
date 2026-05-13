using Domain.Enuns;

namespace Domain.Entities;

public class Opportunity
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string ActionPlan { get; set; }
    public DateTime? ActionPlanGeneratedAt { get; set; }
    public int LeadId { get; set; }
    public Lead Lead { get; set; }
    public int? OwnerId { get; set; }
    public Owner Owner { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public OpportunityStage Stage { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpectedCloseDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    public Opportunity()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }
    public void Deactivate()
    {
        IsActive = false;
    }
    public void Activate()
    {
        IsActive = true;
    }
    public void UpdateActionPlan(string actionPlan)
    {
        ActionPlan = actionPlan;
        ActionPlanGeneratedAt = DateTime.UtcNow;
    }
}