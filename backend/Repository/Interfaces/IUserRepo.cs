using Domain.Entities;

public interface IUserRepo
{
    Task<int> AddAsync(User user);
    Task<User> GetByIdAsync(int idUser);
    Task<User> GetByEmailAsync(string emailUser);
    Task<PagedResult<User>> GetPagedAsync(bool? isActive, int pagina, int quantidadePorPagina);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<User> GetByEmailWithGroupAsync(string emailUser);
    Task<User> GetActiveByIdAsync(int idUser);
}