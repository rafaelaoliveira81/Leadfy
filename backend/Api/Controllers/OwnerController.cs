using Microsoft.AspNetCore.Mvc;
using Application.DTO;

/// <summary>
/// Controller responsável pelos endpoints de gerenciamento de responsáveis.
/// </summary>
[ApiController]
[Route("api/owners")]
public class OwnerController : ControllerBase
{
    private readonly IOwnerApp _ownerApp;

    /// <summary>
    /// Inicializa uma nova instância do controller de responsáveis.
    /// </summary>
    /// <param name="ownerApp">Serviço de aplicação responsável pelas operações de owner.</param>
    public OwnerController(IOwnerApp ownerApp)
    {
        _ownerApp = ownerApp;
    }

    /// <summary>
    /// Adiciona um novo responsável ao sistema.
    /// </summary>
    /// <param name="request">Dados necessários para criação do responsável.</param>
    /// <returns>
    /// Retorna status 201 com o identificador do responsável criado.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Add([FromBody] OwnerRequest request)
    {
        try
        {
            var idOwner = await _ownerApp.AddAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = idOwner }, new { id = idOwner });
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
    /// Obtém um responsável pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do responsável.</param>
    /// <returns>
    /// Retorna status 200 com os dados do responsável encontrado.
    /// Retorna status 404 quando o responsável não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var owner = await _ownerApp.GetByIdAsync(id);

            return Ok(owner);
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
    /// Obtém responsáveis cadastrados filtrando por status de ativação.
    /// </summary>
    /// <param name="isActive">Filtra por status de ativação (opcional).</param>
    /// <returns>
    /// Retorna status 200 com a coleção de responsáveis.
    /// Retorna status 400 quando os parâmetros informados são inválidos.
    /// Retorna status 404 quando nenhum responsável é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Get([FromQuery] bool? isActive)
    {
        try
        {
            var owners = await _ownerApp.GetAllAsync(isActive);

            return Ok(owners);
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
    /// Atualiza os dados cadastrais de um responsável.
    /// </summary>
    /// <param name="ownerRequest">Dados atualizados do responsável.</param>
    /// <returns>
    /// Retorna status 204 quando a atualização é realizada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando o responsável não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] OwnerRequest ownerRequest)
    {
        try
        {
            await _ownerApp.UpdateAsync(ownerRequest);

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
    /// Remove um responsável do sistema.
    /// </summary>
    /// <param name="id">Identificador do responsável a ser removido.</param>
    /// <returns>
    /// Retorna status 204 quando a exclusão é realizada com sucesso.
    /// Retorna status 404 quando o responsável não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        try
        {
            await _ownerApp.DeleteAsync(id);

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
    /// Desativa um responsável.
    /// </summary>
    /// <param name="id">Identificador do responsável a ser desativado.</param>
    /// <returns>
    /// Retorna status 204 quando a desativação é realizada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando o responsável não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPatch("{id:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Deactivate([FromRoute] int id)
    {
        try
        {
            await _ownerApp.DeactivateAsync(id);

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
    /// Ativa um responsável.
    /// </summary>
    /// <param name="id">Identificador do responsável a ser ativado.</param>
    /// <returns>
    /// Retorna status 204 quando a ativação é realizada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando o responsável não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPatch("{id:int}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Activate([FromRoute] int id)
    {
        try
        {
            await _ownerApp.ActivateAsync(id);

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
}
