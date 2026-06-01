namespace Application.DTO;

public class PromptResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}