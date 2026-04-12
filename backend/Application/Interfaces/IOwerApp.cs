using Domain.Entities;

public interface IOwerApp
{
    Task<int> AddAsync(Ower ower);
    Task<Ower> GetByIdAsync(int idOwer);
    Task<IEnumerable<Ower>> GetByNameContainingAsync(string nameOwer);
    Task<IEnumerable<Ower>> GetAllAsync();
    Task<IEnumerable<Ower>> GetAllByStatusAsync(bool statusOwer);
    Task UpdateAsync(Ower ower);
    Task DeleteAsync(int idOwer);
    Task DeactivateAsync(int idOwer);
    Task ActivateAsync(int idOwer);
}