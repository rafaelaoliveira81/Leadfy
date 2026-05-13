namespace Domain.Entities;

public class User
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; private set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Owner> Owners { get; set; }
    public ICollection<Interaction> Interactions { get; set; }
    public ICollection<PasswordRecovery> PasswordRecoveries { get; set; } //Relacionamento 1:N com PasswordRecovery

    public User()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        Owners = new List<Owner>();
        Interactions = new List<Interaction>();
        PasswordRecoveries = new List<PasswordRecovery>();
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