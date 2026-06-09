using Application.DTO;

public interface IPromptApp
{
    Task<string> AddAsync(PromptRequest request);
    Task<PromptResponse> GetByIdAsync(string idPrompt);
    Task<PromptPagedResponse> GetAllAsync(bool? status, int pagina, int quantidadePorPagina);
    Task UpdateAsync(PromptRequest request);
    Task DeleteAsync(string idPrompt);
    Task DeactivateAsync(string idPrompt);
    Task ActivateAsync(string idPrompt);
    Task<PromptOptimizeDTO> OptimizePromptAsync(PromptOptimizeDTO prompt);
}
