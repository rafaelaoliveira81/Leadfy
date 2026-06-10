using Application.DTO;

public interface IUserApp
{
    Task<string> AddAsync(UserRequest user);
    Task<UserResponse> GetByIdAsync(string idUser);
    Task<UserResponse> GetByEmailAsync(string emailUser);
    Task<UserPagedResponse> GetAllAsync(bool? isActive, int pagina, int quantidadePorPagina);
    Task UpdateAsync(string id, UserRequest user);
    Task DeleteAsync(string idUser);
    Task DeactivateAsync(string idUser);
    Task ActivateAsync(string idUser);
    Task UpdatePasswordAsync(string id, UserUpdatePasswordRequest request);
}