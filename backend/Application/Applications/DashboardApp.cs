using Application.DTO;
using Domain.DTO;
using System.Text.Json;
namespace Application;

public class DashboardApp : IDashboardApp
{
    private readonly IDashboardRepo _dashboardRepo;

    public DashboardApp(IDashboardRepo dashboardRepo)
    {
        _dashboardRepo = dashboardRepo;
    }

    public async Task<DashboardResponse> GetDashboardDataAsync()
    {
        var dados = await _dashboardRepo.GetDashboardDataAsync();

        var response = new DashboardResponse
        {
            TotalLeads = dados.TotalLeads,
            OpportunitiesInProgress = dados.OpportunitiesInProgress,
            NewOpportunitiesWithoutContact = dados.NewOpportunitiesWithoutContact,
            RevenueForecast = dados.RevenueForecast,
            TotalPipelineValue = dados.TotalPipelineValue,
            ConversionRate = dados.ConversionRate,
            RealRevenue = dados.RealRevenue,
            StageAnalytics = DeserializeStageAnalytics(dados.StageAnalytics)
        };

        return response;
    }

    private static List<DashboardStageDto> DeserializeStageAnalytics(string stageAnalytics)
    {
        if (string.IsNullOrWhiteSpace(stageAnalytics))
        {
            return new List<DashboardStageDto>();
        }

        return JsonSerializer.Deserialize<List<DashboardStageDto>>(stageAnalytics) ?? new List<DashboardStageDto>();
    }
}