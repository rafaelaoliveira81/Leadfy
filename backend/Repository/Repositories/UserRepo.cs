using Microsoft.EntityFrameworkCore;
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

    public async Task<User> GetScopedByIdAsync(Guid userId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<PagedResult<User>> GetPagedAsync(
        bool? isActive,
        int pagina,
        int quantidadePorPagina)
    {
        if (pagina <= 0)
            throw new ArgumentException("Página deve ser maior que zero.");

        if (quantidadePorPagina <= 0)
            throw new ArgumentException("Quantidade por página deve ser maior que zero.");

        var query = _context.Users.AsNoTracking();

        if (isActive.HasValue)
            query = query.Where(u => u.IsActive == isActive.Value);

        var totalRegistros = await query.CountAsync();
        var users = await query
            .OrderBy(u => u.Name)
            .Skip((pagina - 1) * quantidadePorPagina)
            .Take(quantidadePorPagina)
            .ToListAsync();

        return new PagedResult<User>
        {
            TotalRegistros = totalRegistros,
            Dados = users
        };
    }
}