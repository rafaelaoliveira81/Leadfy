using Domain.Entities;

public interface IProductApp
{
    Task<int> AddAsync(Product product);
    Task<Product> GetByIdAsync(int idProduct);
    Task<IEnumerable<Product>> GetByNameContainingAsync(string nameProduct);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetAllByStatusAsync(bool statusProduct);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int idProduct);
    Task DeactivateAsync(int idProduct);
    Task ActivateAsync(int idProduct);
}
