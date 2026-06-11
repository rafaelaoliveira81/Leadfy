using Domain.Entities;

public interface IPromptRepo : IBaseRepository<Prompt>
{
    Task<PagedResult<Prompt>> GetPagedAsync(Guid tenantId, bool? isActive, int pagina, int quantidadePorPagina);
}
