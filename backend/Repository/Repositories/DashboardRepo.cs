using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using Repository.Context;
using Domain.DTO;
using Dapper;

namespace Repository.Repositories;

public class DashboardRepo : IDashboardRepo
{
    private readonly CRMContext _context;
    public DashboardRepo(CRMContext context)
    {
        _context = context;
    }
    protected DbConnection GetConnection()
    {
        var connectionString = _context.Database.GetConnectionString();

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = _context.Database.GetDbConnection().ConnectionString;
        }

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("ConnectionString não foi inicializada no CRMContext.");

        return new SqlConnection(connectionString);
    }

    public async Task<DashboardDto> GetDashboardDataAsync()
    {
        const string query = @"
            SELECT *
            FROM fn_DashboardAnalytics()";

        using var connection = GetConnection();

        return await connection.QueryFirstOrDefaultAsync<DashboardDto>(query) ?? new DashboardDto();
    }
}