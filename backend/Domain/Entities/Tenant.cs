namespace Domain.Entities;

public class Tenant : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<User> Users { get; set; }
    public ICollection<Lead> Leads { get; set; }
    public ICollection<Product> Products { get; set; }
    public ICollection<Opportunity> Opportunities { get; set; }
    public ICollection<OpportunityActionPlan> OpportunityActionPlans { get; set; }
    public ICollection<Interaction> Interactions { get; set; }
    public ICollection<Prompt> Prompts { get; set; }

    public Tenant()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        Users = new List<User>();
        Leads = new List<Lead>();
        Products = new List<Product>();
        Opportunities = new List<Opportunity>();
        OpportunityActionPlans = new List<OpportunityActionPlan>();
        Interactions = new List<Interaction>();
        Prompts = new List<Prompt>();
    }
}