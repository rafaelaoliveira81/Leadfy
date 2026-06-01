using Domain.DTO;

public interface IDashboardRepo
{
    Task<DashboardDto> GetDashboardDataAsync();
}