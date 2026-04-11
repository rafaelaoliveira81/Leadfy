using Domain.Entities;

public interface IUserRepo
{
    Task<int> AddAsync(User user);
    Task<User> GetByIdAsync(int idUser);
    Task<IEnumerable<User>> GetByNameContainingAsync(string nameUser);
    Task<User> GetByEmailAsync(string emailUser);
    Task<IEnumerable<User>> GetAllAsync();
    Task<IEnumerable<User>> GetAllByStatusAsync(bool statusUser);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}