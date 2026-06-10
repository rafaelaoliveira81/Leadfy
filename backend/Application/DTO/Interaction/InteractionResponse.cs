namespace Application.DTO;

public class InteractionResponse
{
    public string Id { get; set; }
    public string OpportunityId { get; set; }
    public int? FromStage { get; set; }
    public string FromStageName { get; set; }
    public int? ToStage { get; set; }
    public string ToStageName { get; set; }
    public string Description { get; set; }
    public DateTime InteractionDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserName { get; set; }
    public DateTime? NextContactDate { get; set; }
}
