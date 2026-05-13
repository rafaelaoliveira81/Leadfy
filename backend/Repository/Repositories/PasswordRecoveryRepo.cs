using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

/// <summary>
/// Repository responsible for persisting and querying password recovery requests.
/// The Application layer owns the business rules (expiration, single-use, cooldown).
/// Contains only data access logic — no business rules.
/// </summary>
public class PasswordRecoveryRepo : BaseRepo, IPasswordRecoveryRepo
{
    public PasswordRecoveryRepo(CRMContext context) : base(context)
    {
    }

    public async Task<int> AddAsync(PasswordRecovery passwordRecovery)
    {
        _context.PasswordRecoveries.Add(passwordRecovery);
        await _context.SaveChangesAsync();
        return passwordRecovery.ID;
    }

    public async Task<PasswordRecovery> GetByIdAsync(int id)
    {
        return await _context.PasswordRecoveries.FindAsync(id);
    }

    /// <summary>
    /// Returns the recovery record for the given token.
    /// Active and inactive records are both returned so the Application layer can
    /// detect already-consumed tokens and return appropriate error messages.
    /// </summary>
    public async Task<PasswordRecovery> GetByTokenAsync(string token)
    {
        return await _context.PasswordRecoveries
            .FirstOrDefaultAsync(pr => pr.Token == token);
    }

    /// <summary>
    /// Returns the most recent recovery request for the given email.
    /// Used to enforce the minimum interval between password recovery requests.
    /// </summary>
    public async Task<PasswordRecovery> GetLatestByEmailAsync(string email)
    {
        return await _context.PasswordRecoveries
            .Where(pr => pr.Email == email)
            .OrderByDescending(pr => pr.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(PasswordRecovery passwordRecovery)
    {
        _context.PasswordRecoveries.Update(passwordRecovery);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PasswordRecovery passwordRecovery)
    {
        _context.PasswordRecoveries.Remove(passwordRecovery);
        await _context.SaveChangesAsync();
    }
}
