namespace Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Product()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }
}