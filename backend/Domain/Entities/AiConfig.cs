using Dominio.Enums;

namespace Domain.Entities;

public class AiConfig
{
    public int Id { get; private set; }
    public string Title { get; set; }
    public string PromptTemplate { get; set; }
    public AiModelsEnum Model { get; set; }
    public string ApiKeyHash { get; set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public AiConfig(string title, string promptTemplate, AiModelsEnum model, string apiKeyHash)
    {
        Title = title;
        PromptTemplate = promptTemplate;
        Model = model;
        ApiKeyHash = apiKeyHash;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
