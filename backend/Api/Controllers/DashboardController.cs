using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTO;

namespace Api.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de dashboard analítico.
/// </summary>
[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardApp _dashboardApp;

    /// <summary>
    /// Inicializa uma nova instância do controller de dashboard.
    /// </summary>
    /// <param name="dashboardApp">Serviço de aplicação responsável pelas operações de dashboard.</param>
    public DashboardController(IDashboardApp dashboardApp)
    {
        _dashboardApp = dashboardApp;
    }

    /// <summary>
    /// Obtém os dados do dashboard.
    /// </summary>
    /// <returns>
    /// Retorna status 200 com os dados do dashboard.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DashboardResponse>> GetDashboardData()
    {
        try
        {
            var dashboardData = await _dashboardApp.GetDashboardDataAsync();
            return Ok(dashboardData);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}