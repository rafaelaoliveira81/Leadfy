using Domain.Entities;

public interface IOwnerRepo
{
    Task<int> AddAsync(Owner owner);
    Task<Owner> GetByIdAsync(int idOwner);
    Task<IEnumerable<Owner>> GetAllAsync(bool? statusOwner);
    Task UpdateAsync(Owner owner);
    Task DeleteAsync(Owner owner);
}