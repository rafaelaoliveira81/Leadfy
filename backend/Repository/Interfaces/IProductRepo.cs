using Domain.Entities;

public interface IProductRepo
{
    Task<int> AddAsync(Product product);
    Task<Product> GetByIdAsync(int idProduct);
    Task<PagedResult<Product>> GetPagedAsync(bool? statusProduct, int pagina, int quantidadePorPagina);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}
