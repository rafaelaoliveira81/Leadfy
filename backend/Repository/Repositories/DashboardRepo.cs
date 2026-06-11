using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using Repository.Context;
using Domain.DTO;
using Domain.Interface;
using Dapper;
using System.Text.Json;

namespace Repository.Repositories;

public class DashboardRepo : IDashboardRepo
{
    private readonly CRMContext _context;
    private readonly ITenantProvider _tenantProvider;

    public DashboardRepo(CRMContext context, ITenantProvider tenantProvider)
    {
        _context = context;
        _tenantProvider = tenantProvider;
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

    public async Task<DashboardDto> GetDashboardDataAsync(Guid tenantId)
    {
        const string query = @"
            EXEC sp_DashboardSummary @TenantId;
            EXEC sp_DashboardStageAnalytics @TenantId;";

        using var connection = GetConnection();

        using var multi = await connection.QueryMultipleAsync(query, new { TenantId = tenantId });

        var summary = await multi.ReadFirstOrDefaultAsync<DashboardSummaryQueryResult>() ?? new DashboardSummaryQueryResult();
        var stageAnalytics = (await multi.ReadAsync<DashboardStageDto>()).ToList();

        return new DashboardDto
        {
            TotalLeads = summary.TotalLeads,
            OpportunitiesInProgress = summary.OpportunitiesInProgress,
            NewOpportunitiesWithoutContact = summary.NewWithoutContact,
            RevenueForecast = summary.ForecastRevenue,
            TotalPipelineValue = summary.PipelineValue,
            ConversionRate = summary.ConversionRate,
            RealRevenue = summary.RealRevenue,
            StageAnalytics = JsonSerializer.Serialize(stageAnalytics)
        };
    }

    private sealed class DashboardSummaryQueryResult
    {
        public int TotalLeads { get; set; }

        public int OpportunitiesInProgress { get; set; }

        public int NewWithoutContact { get; set; }

        public decimal? ForecastRevenue { get; set; }

        public decimal? PipelineValue { get; set; }

        public decimal? ConversionRate { get; set; }

        public decimal? RealRevenue { get; set; }
    }
}