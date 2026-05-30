using Application.DTO;

public interface ILeadApp
{
    Task<int> AddAsync(LeadRequest lead);
    Task<LeadResponse> GetByIdAsync(int idLead);
    Task<LeadPagedResponse> GetAllAsync(bool? status, int pagina, int quantidadePorPagina);
    Task UpdateAsync(LeadRequest lead);
    Task DeleteAsync(int idLead);
    Task DeactivateAsync(int idLead);
    Task ActivateAsync(int idLead);
}
