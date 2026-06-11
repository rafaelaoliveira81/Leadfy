using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Domain.Interface;
using Repository.Context;

namespace Repository.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepo
{
    private readonly ITenantProvider _tenantProvider;
    public UserRepository(CRMContext context, ITenantProvider tenantProvider) : base(context)
    {
        _tenantProvider = tenantProvider;
    }

    public async Task<User> GetByEmailAsync(string emailUser)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == emailUser);
    }

    public async Task<User> GetByUserNameGlobalAsync(string userName)
    {
        return await _context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<List<User>> GetByEmailAnyTenantAsync(string emailUser)
    {
        return await _context.Users
            .IgnoreQueryFilters()
            .Where(u => u.Email == emailUser)
            .OrderBy(u => u.CreatedAt)
            .ToListAsync();
    }

    public async Task<PagedResult<User>> GetPagedAsync(
        bool? isActive,
        int pagina,
        int quantidadePorPagina)
    {
        var tenantId = _tenantProvider.GetRequiredTenantId();

        using var connection = GetConnection();

        using var multi = await connection.QueryMultipleAsync(
            "sp_GetUsersPaginado",
            new
            {
                TenantId = tenantId,
                Status = isActive.HasValue ? (isActive.Value ? 1 : 0) : (int?)null,
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