using Domain.Entities;
using Application.DTO;

public interface IProductApp
{
    Task<int> AddAsync(ProductRequest request);
    Task<ProductResponse> GetByIdAsync(int idProduct);
    Task<ProductPagedResponse> GetAllAsync(bool? statusProduct, int pagina, int quantidadePorPagina);
    Task UpdateAsync(ProductRequest request);
    Task DeleteAsync(int idProduct);
    Task DeactivateAsync(int idProduct);
    Task ActivateAsync(int idProduct);
}
