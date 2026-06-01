using Application.DTO;
namespace Application;

public class Dashboard : IDashboardApp
{
    private readonly IDashboardRepo _dashboardRepo;

    public Dashboard(IDashboardRepo dashboardRepo)
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
            StageAnalytics = dados.StageAnalytics.ToString()
        };

        return response;
    }
}