using Domain.Entities;

public interface IProductRepo
{
    Task<int> AddAsync(Product product);
    Task<Product> GetByIdAsync(int idProduct);
    Task<IEnumerable<Product>> GetAllAsync(bool? statusProduct);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}
