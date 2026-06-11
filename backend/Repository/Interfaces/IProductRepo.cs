using Domain.Entities;

public interface IProductRepo  : IBaseRepository<Product>
{
    Task<PagedResult<Product>> GetPagedAsync(Guid tenantId, bool? statusProduct, int pagina, int quantidadePorPagina);
}
