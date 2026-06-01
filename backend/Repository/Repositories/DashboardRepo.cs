using Repository.Context;
using Domain.DTO;
using Dapper;

namespace Repository.Repositories;

public class DashboardRepo : BaseRepo, IDashboardRepo
{
    public DashboardRepo(CRMContext context) : base(context)
    {
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