using Domain.Enuns;

namespace Domain.Entities;

public class User
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Owner> Owners { get; set; }
    public ICollection<Interaction> Interactions { get; set; }

    public User()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        Owners = new List<Owner>();
        Interactions = new List<Interaction>();
    }
    public void SetPassword(string password, IPasswordHasher hasher)
    {
        PasswordHash = hasher.Hash(password);
    }
    public bool VerifyPassword(string password, IPasswordHasher hasher)
    {
        return hasher.Verify(PasswordHash, password);
    }
    public void ChangePassword(string currentPassword, string newPassword, IPasswordHasher hasher)
    {
        PasswordHash = hasher.Hash(newPassword);
    }
    public void Deactivate()
    {
        IsActive = false;
    }
    public void Activate()
    {
        IsActive = true;
    }
}