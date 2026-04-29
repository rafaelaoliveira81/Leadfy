using Api.Models.Ai.Request;
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

    /// <summary>
    /// Inicializa uma nova instância do controller de IA.
    /// </summary>
    public AiController(IAiService aiService)
    {
        _aiService = aiService;
    }

    /// <summary>
    /// Envia um prompt avulso ao modelo de IA especificado.
    /// </summary>
    /// <param name="request">Prompt, nome do modelo e chave de API.</param>
    /// <returns>Resposta gerada pelo modelo.</returns>
    /// <response code="200">Resposta gerada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost("completar")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Completar([FromBody] AiPromptRequest request)
    {
        try
        {
            var resposta = await _aiService.GetResponseFromModel(request.Prompt, request.ModelName, request.ApiKey);
            return Ok(resposta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Erro ao obter resposta da IA: {ex.Message}" });
        }
    }
}
