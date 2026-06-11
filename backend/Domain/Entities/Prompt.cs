namespace Domain.Entities;

public class Prompt : IEntity
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; private set; }
    public Tenant Tenant { get; set; }

    public Prompt()
    {
        Id = Guid.NewGuid();
        IsActive = true;
        CreatedAt = DateTime.Now;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}