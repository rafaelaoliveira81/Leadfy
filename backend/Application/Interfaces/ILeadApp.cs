using Domain.Entities;

public interface ILeadApp
{
    Task<int> AddAsync(Lead lead);
    Task<Lead> GetByIdAsync(int idLead);
    Task<IEnumerable<Lead>> GetByNameContainingAsync(string nameLead);
    Task<IEnumerable<Lead>> GetAllAsync();
    Task<IEnumerable<Lead>> GetAllByStatusAsync(bool statusLead);
    Task UpdateAsync(Lead lead);
    Task DeleteAsync(int idLead);
    Task DeactivateAsync(int idLead);
    Task ActivateAsync(int idLead);
}
