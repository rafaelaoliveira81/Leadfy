using Microsoft.AspNetCore.Mvc;
using Application.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de gerenciamento de prompts.
/// </summary>
[ApiController]
[Route("api/prompts")]
public class PromptController : ControllerBase
{
    private readonly IPromptApp _promptApp;

    /// <summary>
    /// Inicializa uma nova instância do controller de prompts.
    /// </summary>
    /// <param name="promptApp">Serviço de aplicação responsável pelas operações de prompt.</param>
    public PromptController(IPromptApp promptApp)
    {
        _promptApp = promptApp;
    }

    /// <summary>
    /// Adiciona um novo prompt ao sistema.
    /// </summary>
    /// <param name="request">Dados necessários para criação do prompt (`PromptRequest`).</param>
    /// <returns>
    /// Retorna status 201 com o identificador do prompt criado.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando recursos relacionados não são encontrados.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Add([FromBody] PromptRequest request)
    {
        try
        {
            var idPrompt = await _promptApp.AddAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = idPrompt }, new { id = idPrompt });
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
    /// Obtém um prompt pelo identificador.
    /// </summary>
    /// <param name="guidId">Identificador do prompt.</param>
    /// <returns>
    /// Retorna status 200 com os dados do prompt encontrado (`PromptResponse` ou similar).
    /// Retorna status 404 quando o prompt não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpGet("{guidId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetById([FromRoute] string guidId)
    {
        try
        {
            var prompt = await _promptApp.GetByIdAsync(guidId);

            return Ok(prompt);
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
    /// Obtém prompts cadastrados, opcionalmente filtrando por status de ativação.
    /// </summary>
    /// <param name="isActive">Filtra por status de ativação (true = ativos, false = inativos, null = todos).</param>
    /// <param name="pagina">Número da página para paginação (padrão: 1).</param>
    /// <param name="quantidadePorPagina">Quantidade de itens por página para paginação (padrão: 10).</param>
    /// <returns>
    /// Retorna status 200 com a coleção paginada de prompts.
    /// Retorna status 400 quando os parâmetros informados são inválidos.
    /// Retorna status 404 quando nenhum prompt é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Get([FromQuery] bool? isActive, [FromQuery] int pagina = 1, [FromQuery] int quantidadePorPagina = 10)
    {
        try
        {
            var prompts = await _promptApp.GetAllAsync(isActive, pagina, quantidadePorPagina);

            return Ok(prompts);
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
    /// Atualiza os dados de um prompt existente.
    /// </summary>
    /// <param name="request">Dados atualizados do prompt (`PromptRequest`).</param>
    /// <returns>
    /// Retorna status 204 quando a atualização é realizada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando o prompt não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] PromptRequest request)
    {
        try
        {
            await _promptApp.UpdateAsync(request);

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
    /// Remove um prompt do sistema.
    /// </summary>
    /// <param name="guidId">Identificador do prompt a ser removido.</param>
    /// <returns>
    /// Retorna status 204 quando a exclusão é realizada com sucesso.
    /// Retorna status 404 quando o prompt não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpDelete("{guidId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete([FromRoute] string guidId)
    {
        try
        {
            await _promptApp.DeleteAsync(guidId);

            return NoContent();
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
    /// Desativa um prompt.
    /// </summary>
    /// <param name="guidId">Identificador do prompt a ser desativado.</param>
    /// <returns>
    /// Retorna status 204 quando a desativação é realizada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando o prompt não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPatch("{guidId}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Deactivate([FromRoute] string guidId)
    {
        try
        {
            await _promptApp.DeactivateAsync(guidId);

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
    /// Ativa um prompt.
    /// </summary>
    /// <param name="guidId">Identificador do prompt a ser ativado.</param>
    /// <returns>
    /// Retorna status 204 quando a ativação é realizada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando o prompt não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPatch("{guidId}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Activate([FromRoute] string guidId)
    {
        try
        {
            await _promptApp.ActivateAsync(guidId);

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
    /// Otimiza o prompt com IA.
    /// </summary>
    /// <param name="prompt">Texto do prompt a ser otimizado.</param>
    /// <returns>
    /// Retorna status 200 com o prompt otimizado.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPost("optimize")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> OptimizePrompt([FromBody] PromptOptimizeDTO prompt)
    {
        try
        {
            var optimizedPrompt = await _promptApp.OptimizePromptAsync(prompt);

            return Ok(new { optimizedPrompt });
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
