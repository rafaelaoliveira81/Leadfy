using Application.DTO;

public interface IDashboardApp
{
    Task<DashboardResponse> GetDashboardDataAsync();
}