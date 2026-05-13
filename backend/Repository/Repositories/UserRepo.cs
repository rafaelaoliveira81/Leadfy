using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class UserRepository : BaseRepo, IUserRepo
{
    public UserRepository(CRMContext context) : base(context)
    {
    }
    public async Task<int> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user.ID;
    }
    public async Task<User> GetByIdAsync(int idUser)
    {
        return await _context.Users.FindAsync(idUser);
    }
    public async Task<User> GetByEmailAsync(string emailUser)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == emailUser);
    }
    public async Task<IEnumerable<User>> GetByNameContainingAsync(string nameUser)
    {
        return await _context.Users
            .Where(u => EF.Functions.Like(u.Name, $"%{nameUser}%"))
            .ToListAsync();
    }
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
    public async Task<IEnumerable<User>> GetAllByStatusAsync(bool statusUser)
    {
        return await _context.Users
            .Where(u => u.IsActive == statusUser)
            .ToListAsync();
    }
    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Returns an active user by email for JWT login.
    /// Returns null when no active user matches the email.
    /// </summary>
    public async Task<User> GetByEmailWithGroupAsync(string emailUser)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == emailUser && u.IsActive);
    }

    /// <summary>
    /// Returns an active user by ID. Returns null when the user is inactive.
    /// </summary>
    public async Task<User> GetActiveByIdAsync(int idUser)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.ID == idUser && u.IsActive);
    }
}