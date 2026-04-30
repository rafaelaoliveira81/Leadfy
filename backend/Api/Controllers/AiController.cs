using Api.Models.Ai.Request;
using Application;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Controller responsável por endpoints avulsos de interação direta com modelos de IA.
/// Para geração de planos de ação de leads, utilize <c>/api/ai-config/{id}/generate-action-plan/{leadId}</c>.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AiController : ControllerBase
{
    private readonly IAiService _aiService;
    private readonly IAiConfigApp _aiConfigApp;

    /// <summary>
    /// Inicializa uma nova instância do controller de IA.
    /// </summary>
    public AiController(IAiService aiService, IAiConfigApp aiConfigApp)
    {
        _aiService = aiService;
        _aiConfigApp = aiConfigApp;
    }

    /// <summary>
    /// Envia um prompt avulso ao modelo de IA definido pela configuração informada.
    /// </summary>
    /// <param name="request">Prompt e ID da configuração de IA a ser utilizada.</param>
    /// <returns>Resposta gerada pelo modelo.</returns>
    /// <response code="200">Resposta gerada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="404">Configuração não localizada.</response>
    [HttpPost("completar")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Completar([FromBody] AiPromptRequest request)
    {
        try
        {
            var config = await _aiConfigApp.GetByIdAsync(request.ConfigId);
            var plainApiKey = _aiConfigApp.DecryptApiKey(config.ApiKeyHash);

            var resposta = await _aiService.GetResponseFromModel(request.Prompt, config.ModelName, plainApiKey);
            return Ok(resposta);
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
            return StatusCode(500, new { message = $"Erro ao obter resposta da IA: {ex.Message}" });
        }
    }
}
