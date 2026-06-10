using Domain.Entities;

public interface IUserRepo : IBaseRepository<User>
{
    Task<User> GetByEmailAsync(string emailUser);
    Task<User> GetByUserNameGlobalAsync(string userName);
    Task<List<User>> GetByEmailAnyTenantAsync(string emailUser);
    Task<User> GetScopedByIdAsync(Guid userId);
    Task<PagedResult<User>> GetPagedAsync(bool? isActive, int pagina, int quantidadePorPagina);
}