using Application.DTO;

public interface ILeadApp
{
    Task<string> AddAsync(LeadRequest lead, string idUser);
    Task<LeadResponse> GetByIdAsync(string idLead);
    Task<LeadPagedResponse> GetAllAsync(bool? status, int pagina, int quantidadePorPagina);
    Task UpdateAsync(LeadRequest lead);
    Task DeleteAsync(string idLead);
    Task DeactivateAsync(string idLead);
    Task ActivateAsync(string idLead);
}
