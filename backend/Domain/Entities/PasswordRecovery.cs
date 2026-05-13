namespace Domain.Entities;

/// <summary>
/// Represents a password recovery request.
/// A token is valid for 10 minutes and can only be used once.
/// </summary>
public class PasswordRecovery
{
    public int ID { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// When false the token has already been consumed or explicitly invalidated.
    /// </summary>
    public bool IsActive { get; set; }

    public User User { get; set; }

    public PasswordRecovery()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddMinutes(10);
    }

    /// <summary>Returns true when the token has passed its expiration time.</summary>
    public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

    /// <summary>Marks the token as consumed so it cannot be reused.</summary>
    public void MarkAsUsed() => IsActive = false;
}
