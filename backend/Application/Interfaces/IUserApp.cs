using Application.DTO;
using Domain.Entities;

public interface IUserApp
{
    Task<int> AddAsync(UserRequest user);
    Task<UserResponse> GetByIdAsync(int idUser);
    Task<UserResponse> GetByEmailAsync(string emailUser);
    Task<UserPagedResponse> GetAllAsync(bool? isActive, int pagina, int quantidadePorPagina);
    Task UpdateAsync(int id, UserRequest user);
    Task DeleteAsync(int idUser);
    Task DeactivateAsync(int idUser);
    Task ActivateAsync(int idUser);
    Task UpdatePasswordAsync(int id, UserUpdatePasswordRequest request);
}