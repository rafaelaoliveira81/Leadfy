using Domain.Entities;

public interface IOwnerApp
{
    Task<int> AddAsync(Owner owner);
    Task<Owner> GetByIdAsync(int idOwner);
    Task<IEnumerable<Owner>> GetAllAsync(bool? statusOwner);
    Task UpdateAsync(Owner owner);
    Task DeleteAsync(int idOwner);
    Task DeactivateAsync(int idOwner);
    Task ActivateAsync(int idOwner);
}