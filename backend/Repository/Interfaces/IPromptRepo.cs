using Domain.Entities;

public interface IPromptRepo
{
    Task<int> AddAsync(Prompt prompt);
    Task<Prompt> GetByIdAsync(int idPrompt);
    Task<PagedResult<Prompt>> GetPagedAsync(bool? isActive, int pagina, int quantidadePorPagina);
    Task UpdateAsync(Prompt prompt);
    Task DeleteAsync(Prompt prompt);
}
