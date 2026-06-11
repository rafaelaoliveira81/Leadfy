using Domain.Entities;

public interface IUserRepo : IBaseRepository<User>
{
    Task<User> GetByEmailAsync(string emailUser);
    Task<User> GetByUserNameGlobalAsync(string userName);
    Task<List<User>> GetByEmailAnyTenantAsync(string emailUser);
    Task<PagedResult<User>> GetPagedAsync(Guid tenantId, bool? isActive, int pagina, int quantidadePorPagina);
}