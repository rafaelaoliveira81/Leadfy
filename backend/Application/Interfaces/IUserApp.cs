using Domain.Entities;

public interface IUserApp
{
    Task<int> AddAsync(User user, string password);
    Task<User> GetByIdAsync(int idUser);
    Task<User> GetByEmailAsync(string emailUser);
    Task<IEnumerable<User>> GetByNameContainingAsync(string nameUser);
    Task<IEnumerable<User>> GetAllAsync();
    Task<IEnumerable<User>> GetAllByStatusAsync(bool statusUser);
    Task UpdateAsync(User user);
    Task DeleteAsync(int idUser);
    Task DeactivateAsync(int idUser);
    Task ActivateAsync(int idUser);
    Task UpdatePasswordAsync(int userId, string currentPassword, string newPassword);
}