namespace Domain.DTO;

public class DashboardDto
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