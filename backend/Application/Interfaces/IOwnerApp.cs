using Domain.Entities;

public interface IOwnerApp
{
    Task<int> AddAsync(Owner owner);
    Task<Owner> GetByIdAsync(int idOwner);
    Task<IEnumerable<Owner>> GetByNameContainingAsync(string nameOwner);
    Task<IEnumerable<Owner>> GetAllAsync();
    Task<IEnumerable<Owner>> GetAllByStatusAsync(bool statusOwner);
    Task UpdateAsync(Owner owner);
    Task DeleteAsync(int idOwner);
    Task DeactivateAsync(int idOwner);
    Task ActivateAsync(int idOwner);
}