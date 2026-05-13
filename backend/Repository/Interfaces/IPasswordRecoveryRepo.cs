using Domain.Entities;

public interface IPasswordRecoveryRepo
{
    Task<int> AddAsync(PasswordRecovery passwordRecovery);
    Task<PasswordRecovery> GetByIdAsync(int id);

    /// <summary>
    /// Returns the recovery request for the given token (active or not).
    /// The Application layer decides whether the token is still valid.
    /// </summary>
    Task<PasswordRecovery> GetByTokenAsync(string token);

    /// <summary>
    /// Returns the most recent recovery request for the given email.
    /// Used to enforce the 1-minute cooldown between requests.
    /// </summary>
    Task<PasswordRecovery> GetLatestByEmailAsync(string email);

    Task UpdateAsync(PasswordRecovery passwordRecovery);
    Task DeleteAsync(PasswordRecovery passwordRecovery);
}
