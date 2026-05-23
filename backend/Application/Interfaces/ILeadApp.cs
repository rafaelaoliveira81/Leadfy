using Application.DTO;
using Domain.Entities;
using Repository.Repositories;

public interface ILeadApp
{
    Task<int> AddAsync(LeadRequest lead);
    Task<LeadResponse> GetByIdAsync(int idLead);
    Task<IEnumerable<LeadResponse>> GetAllAsync(bool? statusLead);
    Task UpdateAsync(LeadRequest lead);
    Task DeleteAsync(int idLead);
    Task DeactivateAsync(int idLead);
    Task ActivateAsync(int idLead);
}
