using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Repository.Context;

namespace Repository.Repositories;

public class InteractionRepo : BaseRepo, IInteractionRepo
{
    public InteractionRepo(CRMContext context) : base(context)
    {
    }
    public async Task<int> AddAsync(Interaction interaction)
    {
        var query = "sp_CreateInteraction";
        var parameters = new
        {
            OpportunityId = interaction.OpportunityId,
            FromStage = (int?)interaction.FromStage,
            ToStage = (int?)interaction.ToStage,
            Description = interaction.Description,
            UserID = interaction.UserId,
            NextContactDate = interaction.NextContactDate,
            InteractionDate = interaction.InteractionDate
        };

        using (var connection = GetConnection())
        {
            return await connection.ExecuteScalarAsync<int>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<Interaction> GetByIdAsync(int idInteraction)
    {
        var query = "sp_GetInteractionById";
        var parameters = new { ID = idInteraction };

        using (var connection = GetConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Interaction>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<IEnumerable<Interaction>> GetAllByOpportunityIdAsync(int opportunityId)
    {
        return await _context.Interactions
            .Where(i => i.OpportunityId == opportunityId)
            .Include(i => i.User)
            .OrderByDescending(i => i.InteractionDate)
            .ThenByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Interaction>> GetLastInteractionsByOpportunityIdAsync(int opportunityId)
    {
        return await _context.Interactions
            .Where(i => i.OpportunityId == opportunityId)
            .Include(i => i.User)
            .OrderByDescending(i => i.InteractionDate)
            .ThenByDescending(i => i.CreatedAt)
            .Take(3)
            .ToListAsync();
    }

    public async Task UpdateAsync(Interaction interaction)
    {
        var query = "sp_UpdateInteraction";
        var parameters = new
        {
            ID = interaction.Id,
            OpportunityId = interaction.OpportunityId,
            FromStage = (int?)interaction.FromStage,
            ToStage = (int?)interaction.ToStage,
            Description = interaction.Description,
            UserID = interaction.UserId,
            NextContactDate = interaction.NextContactDate,
            InteractionDate = interaction.InteractionDate
        };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task DeleteAsync(Interaction interaction)
    {
        var query = "sp_DeleteInteraction";
        var parameters = new { ID = interaction.Id };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}