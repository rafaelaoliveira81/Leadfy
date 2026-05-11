using Domain.Entities;

public interface IPermissionRepo
{
    Task<int> AddAsync(Permission permission);
    Task<Permission> GetByIdAsync(int id);
    Task<Permission> GetByNameAsync(string name);
    Task<IEnumerable<Permission>> GetAllAsync();
    Task<IEnumerable<Permission>> GetAllActiveAsync();
    Task UpdateAsync(Permission permission);
    Task DeleteAsync(Permission permission);
}
