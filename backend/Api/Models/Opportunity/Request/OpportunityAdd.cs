namespace Models.Request;

public class OpportunityAdd
{
    public int LeadId { get; set; }
    public int? OwnerId { get; set; }
    public int ProductId { get; set; }
    public int Stage { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpectedCloseDate { get; set; }
}
