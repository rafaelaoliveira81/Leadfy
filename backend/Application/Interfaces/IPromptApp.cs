using Application.DTO;

public interface IPromptApp
{
    Task<int> AddAsync(PromptRequest request);
    Task<PromptResponse> GetByIdAsync(int idPrompt);
    Task<PromptPagedResponse> GetAllAsync(bool? status, int pagina, int quantidadePorPagina);
    Task UpdateAsync(PromptRequest request);
    Task DeleteAsync(int idPrompt);
    Task DeactivateAsync(int idPrompt);
    Task ActivateAsync(int idPrompt);
}
