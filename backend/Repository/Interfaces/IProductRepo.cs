using Domain.Entities;

public interface IProductRepo  : IBaseRepository<Product>
{
    Task<PagedResult<Product>> GetPagedAsync(bool? statusProduct, int pagina, int quantidadePorPagina);
}
