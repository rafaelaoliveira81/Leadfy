namespace Application.DTO;

public class OpportunityUpdate
{
    public string LeadId { get; set; }
    public string ProductId { get; set; }
    public int Stage { get; set; }
    public decimal Amount { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
}
