using Domain.Entities;

public interface IOwerRepo
{
    Task<int> AddAsync(Ower ower);
    Task<Ower> GetByIdAsync(int idOwer);
    Task<IEnumerable<Ower>> GetByNameContainingAsync(string nameOwer);
    Task<IEnumerable<Ower>> GetAllAsync();
    Task<IEnumerable<Ower>> GetAllByStatusAsync(bool statusOwer);
    Task UpdateAsync(Ower ower);
    Task DeleteAsync(Ower ower);
}