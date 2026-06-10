using Domain.Entities;

public interface ILeadRepo : IBaseRepository<Lead>
{
    Task<PagedResult<Lead>> GetPagedAsync(bool? status, int pagina, int quantidadePorPagina);
}
