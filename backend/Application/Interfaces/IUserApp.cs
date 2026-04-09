using Domain.Entities;

public interface IUserApp
{
    Task<int> AddAsync(User user);
    Task<User> GetByIdAsync(int idUser);
    Task<User> GetByEmailAsync(string emailUser);
    Task<User> GetByNameAsync(string nameUser);
    Task<IEnumerable<User>> GetAllAsync();
    Task<IEnumerable<User>> GetAllByStatusAsync(bool statusUser);
    Task UpdateAsync(User user);
    Task DeleteAsync(int idUser);
    Task DeactivateAsync(int idUser);
    Task ActivateAsync(int idUser);
    Task UpdatePasswordAsync(User user, string currentPassword);
}