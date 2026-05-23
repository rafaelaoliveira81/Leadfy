using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiLoginRequest = Models.Request.LoginRequest;
using ApiLoginResponse = Models.Response.LoginResponse;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AutenticarController : ControllerBase
{
    private readonly IAuthenticationApp _authenticationApp;

    public AutenticarController(IAuthenticationApp authenticationApp)
    {
        _authenticationApp = authenticationApp;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiLoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiLoginResponse>> Login([FromBody] ApiLoginRequest request)
    {
        try
        {
            var response = await _authenticationApp.LoginAsync(new Application.DTO.LoginRequest
            {
                Email = request.Email,
                Password = request.Password
            });

            return Ok(new ApiLoginResponse
            {
                Success = response.Success,
                Message = response.Message,
                Token = response.Token,
                Name = response.Name
            });
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