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

    /// <summary>
    /// Returns an active user by email for JWT login.
    /// </summary>
    Task<User> GetByEmailWithGroupAsync(string emailUser);

    /// <summary>
    /// Returns an active user by ID. Returns null when the user is inactive.
    /// Used by permission validators in the Application layer.
    /// </summary>
    Task<User> GetActiveByIdAsync(int idUser);
}