namespace Application.DTO;

public class OpportunityUpdate
{
    public int LeadId { get; set; }
    public int ProductId { get; set; }
    public int Stage { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpectedCloseDate { get; set; }
}
