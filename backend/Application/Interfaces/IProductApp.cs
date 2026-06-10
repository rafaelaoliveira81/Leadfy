using Domain.Entities;
using Application.DTO;

public interface IProductApp
{
    Task<string> AddAsync(ProductRequest request);
    Task<ProductResponse> GetByIdAsync(string idProduct);
    Task<ProductPagedResponse> GetAllAsync(bool? statusProduct, int pagina, int quantidadePorPagina);
    Task UpdateAsync(ProductRequest request);
    Task DeleteAsync(string idProduct);
    Task DeactivateAsync(string idProduct);
    Task ActivateAsync(string idProduct);
}
