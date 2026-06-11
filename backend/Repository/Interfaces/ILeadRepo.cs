using Domain.Entities;

public interface ILeadRepo : IBaseRepository<Lead>
{
    Task<PagedResult<Lead>> GetPagedAsync(Guid tenantId, bool? status, int pagina, int quantidadePorPagina);
}
