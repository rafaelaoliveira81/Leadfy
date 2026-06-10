namespace Domain.Entities;

public class Lead : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; private set; }
    public Lead()
    {
        Id = Guid.NewGuid();
        IsActive = true;
        CreatedAt = DateTime.Now;
    }
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}