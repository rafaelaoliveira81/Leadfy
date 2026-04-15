using Domain.Enuns;

namespace Domain.Entities;

public class Opportunity
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int LeadId { get; set; }
    public Lead Lead { get; set; }
    public int? OwnerId { get; set; }
    public Ower Owner { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public OpportunityStage Stage { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpectedCloseDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    /// <summary>
    /// Construtor padrão.
    /// Inicializa a opportunity como ativa e define a data de criação.
    /// </summary>
    public Opportunity()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Desativa a opportunity no sistema.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Ativa a opportunity no sistema.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }
}