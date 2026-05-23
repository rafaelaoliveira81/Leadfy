namespace Domain.Entities;

public class PasswordRecovery
{
    public int ID { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }

    public bool IsActive { get; set; }

    public User User { get; set; }

    public PasswordRecovery()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddMinutes(10);
    }

    public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

    public void MarkAsUsed() => IsActive = false;
}
