namespace Application.DTO;

public class LeadPagedResponse
{
    public int TotalRegistros { get; set; }
    public List<LeadResponse> Dados { get; set; } = new();
}
