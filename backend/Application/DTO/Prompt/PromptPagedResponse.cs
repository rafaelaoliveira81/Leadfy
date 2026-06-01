namespace Application.DTO;

public class PromptPagedResponse
{
    public int TotalRegistros { get; set; }
    public List<PromptResponse> Dados { get; set; } = new();
}
