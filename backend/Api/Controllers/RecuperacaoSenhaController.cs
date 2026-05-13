using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Request;

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

    [AllowAnonymous]
    [HttpPost("request")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RequestRecovery([FromBody] ForgotPasswordRequest request)
    {
        await _passwordRecoveryApp.RequestPasswordRecoveryAsync(new Application.DTO.ForgotPasswordRequest
        {
            Email = request.Email
        });

        return Ok(new { message = "Solicitação de recuperação enviada com sucesso." });
    }

    [AllowAnonymous]
    [HttpPost("validate-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ValidateToken([FromBody] ValidateTokenRequest request)
    {
        var recovery = await _passwordRecoveryApp.ValidateTokenAsync(new Application.DTO.ValidateTokenRequest
        {
            Token = request.Token
        });

        return Ok(new
        {
            valid = true,
            expiresAt = recovery.ExpiresAt,
            email = recovery.Email
        });
    }

    [AllowAnonymous]
    [HttpPost("reset")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        await _passwordRecoveryApp.ResetPasswordAsync(new Application.DTO.ResetPasswordRequest
        {
            Token = request.Token,
            NewPassword = request.NewPassword,
            ConfirmPassword = request.ConfirmPassword
        });

        return Ok(new { message = "Senha redefinida com sucesso." });
    }
}