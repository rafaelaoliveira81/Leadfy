
using Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/password-recovery")]
public class RecuperacaoSenhaController : ControllerBase
{
    private readonly IPasswordRecoveryApp _passwordRecoveryApp;

    public RecuperacaoSenhaController(IPasswordRecoveryApp passwordRecoveryApp)
    {
        _passwordRecoveryApp = passwordRecoveryApp;
    }

    [HttpPost("request")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RequestRecovery([FromBody] ForgotPasswordRequest request)
    {
        try
        {
            await _passwordRecoveryApp.RequestPasswordRecoveryAsync(request);

            return Ok(new { message = "Solicitação de recuperação enviada com sucesso." });
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

    [HttpPost("validate-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ValidateToken([FromBody] ValidateTokenRequest request)
    {
        try
        {
            var recovery = await _passwordRecoveryApp.ValidateTokenAsync(request);

            return Ok(new
            {
                valid = true,
                expiresAt = recovery.ExpiresAt,
                email = recovery.Email
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("reset")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            await _passwordRecoveryApp.ResetPasswordAsync(request);

            return Ok(new { message = "Senha redefinida com sucesso." });
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