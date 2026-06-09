namespace Application.DTO;

public class InteractionRequest
{
    public string Description { get; set; }
    public string UserId { get; set; }
    public int? FromStage { get; set; }
    public int? ToStage { get; set; }
    public DateTime? InteractionDate { get; set; }
    public DateTime? NextContactDate { get; set; }
}
