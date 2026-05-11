using Domain.Entities;

public interface IUserGroupRepo
{
    Task<int> AddAsync(UserGroup userGroup);
    Task<UserGroup> GetByIdAsync(int id);
    Task<UserGroup> GetByNameAsync(string name);
    Task<IEnumerable<UserGroup>> GetAllAsync();
    Task<IEnumerable<UserGroup>> GetAllActiveAsync();
    Task UpdateAsync(UserGroup userGroup);
    Task DeleteAsync(UserGroup userGroup);
}
