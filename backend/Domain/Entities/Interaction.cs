using Domain.Enuns;

namespace Domain.Entities;

public class Interaction
{
    public int Id { get; set; }
    public int CrmEntityId { get; set; }
    public CrmEntityType CrmEntityType { get; set; }
    public int? FromStage { get; set; }
    public int? ToStage { get; set; }
    public string Description { get; set; }
    public DateTime InteractionDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime? NextContactDate { get; set; }
    public Interaction()
    {
        CreatedAt = DateTime.UtcNow;
        InteractionDate = DateTime.UtcNow;
    }
}