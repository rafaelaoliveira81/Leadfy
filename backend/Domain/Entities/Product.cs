namespace Domain.Entities;

public class Product : IEntity
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; private set; }
    public Tenant Tenant { get; set; }

    public Product()
    {
        Id = Guid.NewGuid();
        IsActive = true;
        CreatedAt = DateTime.Now;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}