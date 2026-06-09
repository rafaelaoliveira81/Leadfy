using Domain.Entities;

public interface IPromptRepo : IBaseRepository<Prompt>
{
    Task<PagedResult<Prompt>> GetPagedAsync(bool? isActive, int pagina, int quantidadePorPagina);
}
