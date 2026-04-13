using Domain.Entities;

public interface IProductRepo
{
    Task<int> AddAsync(Product product);
    Task<Product> GetByIdAsync(int idProduct);
    Task<IEnumerable<Product>> GetByNameContainingAsync(string nameProduct);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetAllByStatusAsync(bool statusProduct);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}
