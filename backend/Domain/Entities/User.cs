namespace Domain.Entities;

public class User : IEntity
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public Tenant Tenant { get; set; }
    public ICollection<Interaction> Interactions { get; set; }

    public User()
    {
        Id = Guid.NewGuid();
        IsActive = true;
        CreatedAt = DateTime.Now;
        Interactions = new List<Interaction>();
    }
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}