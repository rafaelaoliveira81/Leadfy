using Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de gerenciamento do histórico de interações.
/// </summary>
[ApiController]
[Route("api")]
public class InteractionController : ControllerBase
{
    private readonly IInteractionApp _interactionApp;

    /// <summary>
    /// Inicializa uma nova instância do controller de interações.
    /// </summary>
    /// <param name="interactionApp">Serviço de aplicação responsável pelas operações de interação.</param>
    public InteractionController(IInteractionApp interactionApp)
    {
        _interactionApp = interactionApp;
    }

    /// <summary>
    /// Adiciona uma nova interação ao histórico de uma opportunity.
    /// </summary>
    /// <param name="opportunityId">Identificador da opportunity.</param>
    /// <param name="interactionRequest">Dados necessários para criação da interação.</param>
    /// <returns>
    /// Retorna status 201 com o identificador da interação criada.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando a opportunity ou o usuário não são localizados.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPost("opportunities/{opportunityId:int}/interactions")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AddToOpportunity([FromRoute] int opportunityId, [FromBody] InteractionAdd interactionRequest)
    {
        try
        {
            var interactionId = await _interactionApp.AddToOpportunityAsync(opportunityId, interactionRequest);

            return CreatedAtAction(nameof(GetById), new { id = interactionId }, new { id = interactionId });
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

    /// <summary>
    /// Obtém o histórico de interações de uma opportunity.
    /// </summary>
    /// <param name="opportunityId">Identificador da opportunity.</param>
    /// <returns>
    /// Retorna status 200 com a coleção de interações.
    /// Retorna status 400 quando o identificador informado é inválido.
    /// Retorna status 404 quando a opportunity não é localizada.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpGet("opportunities/{opportunityId:int}/interactions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetByOpportunityId([FromRoute] int opportunityId)
    {
        try
        {
            var interactions = await _interactionApp.GetByOpportunityIdAsync(opportunityId);

            return Ok(interactions);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
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

    /// <summary>
    /// Obtém uma interação pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da interação.</param>
    /// <returns>
    /// Retorna status 200 com os dados da interação encontrada.
    /// Retorna status 400 quando o identificador informado é inválido.
    /// Retorna status 404 quando a interação não é localizada.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpGet("interactions/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var interaction = await _interactionApp.GetByIdAsync(id);

            return Ok(interaction);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
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

    /// <summary>
    /// Remove uma interação do sistema.
    /// </summary>
    /// <param name="id">Identificador da interação a ser removida.</param>
    /// <returns>
    /// Retorna status 204 quando a exclusão é realizada com sucesso.
    /// Retorna status 400 quando o identificador informado é inválido.
    /// Retorna status 404 quando a interação não é localizada.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpDelete("interactions/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        try
        {
            await _interactionApp.DeleteAsync(id);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
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

    /// <summary>
    /// Otimiza a interação com IA.
    /// </summary>
    /// <param name="interaction">Texto do prompt a ser otimizado.</param>
    /// <returns>
    /// Retorna status 200 com o prompt otimizado.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPost("interactions/optimize")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> OptimizePrompt([FromBody] IteractionOptimizeDTO interaction)
    {
        try
        {
            var optimizedInteraction = await _interactionApp.OptimizeInteractionAsync(interaction);

            return Ok(new { optimizedInteraction });
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
}