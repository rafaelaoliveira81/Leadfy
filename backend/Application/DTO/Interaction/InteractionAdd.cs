namespace Application.DTO;

public class InteractionAdd
{
    public string Description { get; set; }
    public int UserId { get; set; }
    public int? FromStage { get; set; }
    public int? ToStage { get; set; }
    public DateTime? InteractionDate { get; set; }
    public DateTime? NextContactDate { get; set; }
}
