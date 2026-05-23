using Domain.Entities;
using Application.DTO;

public interface IProductApp
{
    Task<int> AddAsync(ProductRequest request);
    Task<ProductResponse> GetByIdAsync(int idProduct);
    Task<IEnumerable<ProductResponse>> GetAllAsync(bool? statusProduct);
    Task UpdateAsync(ProductRequest request);
    Task DeleteAsync(int idProduct);
    Task DeactivateAsync(int idProduct);
    Task ActivateAsync(int idProduct);
}
