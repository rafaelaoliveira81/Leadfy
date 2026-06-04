using Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de autenticação.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationApp _authenticationApp;

    /// <summary>
    /// Inicializa uma nova instância do controller de autenticação.
    /// </summary>
    /// <param name="authenticationApp">Serviço de aplicação responsável pelas operações de autenticação.</param>
    public AuthenticationController(IAuthenticationApp authenticationApp)
    {
        _authenticationApp = authenticationApp;
    }

    /// <summary>
    /// Realiza o login do usuário com base nas credenciais informadas.
    /// </summary>
    /// <param name="request">Credenciais utilizadas para autenticar o usuário.</param>
    /// <returns>
    /// Retorna status 200 com os dados de autenticação quando o login é realizado com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 401 quando as credenciais são inválidas.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await _authenticationApp.LoginAsync(request);

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}