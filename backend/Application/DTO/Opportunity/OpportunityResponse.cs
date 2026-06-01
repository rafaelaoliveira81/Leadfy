namespace Application.DTO;

public class OpportunityResponse
{
    public int ID { get; set; }
    public int LeadId { get; set; }
    public string LeadName { get; set; }
    public int? ProductId { get; set; }
    public string ProductName { get; set; }
    public int Stage { get; set; }
    public string StageName { get; set; }
    public string Status { get; set; }
    public decimal Amount { get; set; }
    public int SortOrder { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}
