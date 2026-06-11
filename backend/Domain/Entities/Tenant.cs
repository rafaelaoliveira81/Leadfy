namespace Domain.Entities;

public class Tenant : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<User> Users { get; set; }
    public ICollection<Product> Products { get; set; }

    public Tenant()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        Users = new List<User>();
        Products = new List<Product>();
    }
}