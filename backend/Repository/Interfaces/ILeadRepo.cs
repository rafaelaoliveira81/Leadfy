using Domain.Entities;

public interface ILeadRepo
{
    Task<int> AddAsync(Lead lead);
    Task<Lead> GetByIdAsync(int idLead);
    Task<IEnumerable<Lead>> GetAllAsync(bool? statusLead);
    Task UpdateAsync(Lead lead);
    Task DeleteAsync(Lead lead);
}
