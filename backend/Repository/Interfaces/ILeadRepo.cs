using Domain.Entities;

public interface ILeadRepo
{
    Task<int> AddAsync(Lead lead);
    Task<Lead> GetByIdAsync(int idLead);
    Task<PagedResult<Lead>> GetPagedAsync(bool? status, int pagina, int quantidadePorPagina);
    Task UpdateAsync(Lead lead);
    Task DeleteAsync(Lead lead);
}
