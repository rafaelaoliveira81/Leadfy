using Domain.Entities;

public interface IUserRepo : IBaseRepository<User>
{
    Task<User> GetByEmailAsync(string emailUser);
    Task<PagedResult<User>> GetPagedAsync(bool? isActive, int pagina, int quantidadePorPagina);
}