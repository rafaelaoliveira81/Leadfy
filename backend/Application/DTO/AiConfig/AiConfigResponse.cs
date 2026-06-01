namespace Application.DTO;

public class AiConfigResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string PromptTemplate { get; set; }
    public int Model { get; set; }
    public string ApiKeyMasked { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
