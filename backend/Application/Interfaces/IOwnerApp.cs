using Application.DTO;
using Domain.Entities;

public interface IOwnerApp
{
    Task<int> AddAsync(OwnerRequest request);
    Task<OwnerResponse> GetByIdAsync(int idOwner);
    Task<IEnumerable<OwnerResponse>> GetAllAsync(bool? statusOwner);
    Task UpdateAsync(OwnerRequest request);
    Task DeleteAsync(int idOwner);
    Task DeactivateAsync(int idOwner);
    Task ActivateAsync(int idOwner);
}