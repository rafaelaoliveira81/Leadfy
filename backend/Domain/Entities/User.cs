namespace Domain.Entities;

public class User
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Interaction> Interactions { get; set; }

    public User()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        Interactions = new List<Interaction>();
    }
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}