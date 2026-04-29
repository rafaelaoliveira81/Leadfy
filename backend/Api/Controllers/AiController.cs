using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Application.Interfaces;

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
        var resposta = await _aiService.GetAiResponseAsync(prompt);
        return Ok(resposta);
    }
}