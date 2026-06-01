namespace Application.DTO;

public class AiConfigRequest
{
    public string Title { get; set; }
    public string PromptTemplate { get; set; }
    public int Model { get; set; }
    public string ApiKey { get; set; }
}