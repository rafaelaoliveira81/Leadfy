using Domain.DTO;

namespace Application.DTO;

public class DashboardResponse
{
    public int TotalLeads { get; set; }
    public int OpportunitiesInProgress { get; set; }
    public int NewOpportunitiesWithoutContact { get; set; }
    public decimal? RevenueForecast { get; set; }
    public decimal? TotalPipelineValue { get; set; }
    public decimal? ConversionRate { get; set; }
    public decimal? RealRevenue { get; set; }
    public List<DashboardStageDto> StageAnalytics { get; set; } = new();
}