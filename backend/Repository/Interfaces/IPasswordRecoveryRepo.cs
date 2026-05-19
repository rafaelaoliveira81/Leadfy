using Domain.Entities;

public interface IPasswordRecoveryRepo
{
    Task<int> AddAsync(PasswordRecovery passwordRecovery);
    Task<PasswordRecovery> GetByIdAsync(int id);
    Task<PasswordRecovery> GetByTokenAsync(string token);
    Task<PasswordRecovery> GetLatestByEmailAsync(string email);
    Task UpdateAsync(PasswordRecovery passwordRecovery);
    Task DeleteAsync(PasswordRecovery passwordRecovery);
}
