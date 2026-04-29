using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de interação com a inteligência artificial.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AiController : ControllerBase
{
    private readonly IAiService _aiService;

    /// <summary>
    /// Inicializa uma nova instância do controller de interações.
    /// </summary>
    /// <param name="aiService">Serviço de aplicação responsável pelas operações de interação com a inteligência artificial.</param>
    public AiController(IAiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("completar")]
    public async Task<IActionResult> Completar([FromBody] string prompt)
    {
        try
        {
            var resposta = await _aiService.GetResponseFromModel(prompt);
            return Ok(resposta);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao obter resposta da IA: {ex.Message}");
        }
    }
}