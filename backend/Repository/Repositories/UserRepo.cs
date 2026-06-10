using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepo
{
    public UserRepository(CRMContext context) : base(context)
    {
    }

    public async Task<User> GetByEmailAsync(string emailUser)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == emailUser);
    }

    public async Task<PagedResult<User>> GetPagedAsync(
        bool? isActive,
        int pagina,
        int quantidadePorPagina)
    {
        using var connection = GetConnection();

        using var multi = await connection.QueryMultipleAsync(
            "sp_GetUsersPaginado",
            new
            {
                isActive = isActive.HasValue ? (isActive.Value ? 1 : 0) : (int?)null,
                Pagina = pagina,
                QuantidadePorPagina = quantidadePorPagina
            },
            commandType: System.Data.CommandType.StoredProcedure
        );

        var totalRegistros = await multi.ReadFirstAsync<int>();
        var users = (await multi.ReadAsync<User>()).ToList();

        return new PagedResult<User>
        {
            TotalRegistros = totalRegistros,
            Dados = users
        };
    }
}