using Microsoft.AspNetCore.Mvc;
using Models.Request;
using Models.Response;
using Domain.Entities;

[ApiController]
[Route("owers")]
public class OwerController : ControllerBase
{
    private readonly IOwerApp _owerApp;

    /// <summary>
    /// Inicializa uma nova instância do controller de owers.
    /// </summary>
    /// <param name="owerApp">Serviço de aplicação responsável pelas operações de ower.</param>
    public OwerController(IOwerApp owerApp)
    {
        _owerApp = owerApp;
    }

    /// <summary>
    /// Adiciona um novo ower ao sistema.
    /// </summary>
    /// <param name="owerRequest">Dados necessários para criação do ower.</param>
    /// <returns>
    /// Retorna status 201 com o identificador do ower criado.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult> Add([FromBody] OwerAdd owerRequest)
    {
        try
        {
            var ower = new Ower
            {
                Name = owerRequest.Name,
                UserID = owerRequest.UserID
            };

            var idOwer = await _owerApp.AddAsync(ower);

            return CreatedAtAction(nameof(GetById), new { id = idOwer }, new { id = idOwer });
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
    /// Obtém um ower pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do ower.</param>
    /// <returns>
    /// Retorna status 200 com os dados do ower encontrado.
    /// Retorna status 404 quando o ower não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var ower = await _owerApp.GetByIdAsync(id);

            var owerResponse = new OwerResponse
            {
                ID = ower.ID,
                Name = ower.Name,
                UserID = ower.UserID,
                UserName = ower.User.Name,
                IsActive = ower.IsActive,
                CreatedAt = ower.CreatedAt
            };

            return Ok(owerResponse);
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
    /// Obtém owers cadastrados. Permite filtrar por status de ativação ou nome.
    /// </summary>
    /// <param name="isActive">Filtra por status de ativação (opcional).</param>
    /// <param name="name">Filtra por nome contendo o valor (opcional).</param>
    /// <returns>
    /// Retorna status 200 com a coleção de owers.
    /// Retorna status 400 quando os parâmetros informados são inválidos.
    /// Retorna status 404 quando nenhum ower é localizado na busca por nome.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] bool? isActive, [FromQuery] string name)
    {
        try
        {
            IEnumerable<Ower> owers;

            if (isActive.HasValue)
            {
                owers = await _owerApp.GetAllByStatusAsync(isActive.Value);
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                owers = await _owerApp.GetByNameContainingAsync(name);
            }
            else
            {
                owers = await _owerApp.GetAllAsync();
            }

            var owersResponse = owers.Select(o => new OwerResponse
            {
                ID = o.ID,
                Name = o.Name,
                UserID = o.UserID,
                UserName = o.User.Name,
                IsActive = o.IsActive,
                CreatedAt = o.CreatedAt
            });

            return Ok(owersResponse);
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
    /// Atualiza os dados cadastrais de um ower.
    /// </summary>
    /// <param name="id">Identificador do ower a ser atualizado.</param>
    /// <param name="owerRequest">Dados atualizados do ower.</param>
    /// <returns>
    /// Retorna status 204 quando a atualização é realizada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando o ower não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] OwerUpdate owerRequest)
    {
        try
        {
            var ower = new Ower
            {
                ID = id,
                Name = owerRequest.Name,
                UserID = owerRequest.UserID,
                IsActive = owerRequest.IsActive
            };

            await _owerApp.UpdateAsync(ower);

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
    /// Remove um ower do sistema.
    /// </summary>
    /// <param name="id">Identificador do ower a ser removido.</param>
    /// <returns>
    /// Retorna status 204 quando a exclusão é realizada com sucesso.
    /// Retorna status 404 quando o ower não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        try
        {
            await _owerApp.DeleteAsync(id);

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
    /// Desativa um ower.
    /// </summary>
    /// <param name="id">Identificador do ower a ser desativado.</param>
    /// <returns>
    /// Retorna status 204 quando a desativação é realizada com sucesso.
    /// Retorna status 404 quando o ower não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPatch("{id:int}/deactivate")]
    public async Task<ActionResult> Deactivate([FromRoute] int id)
    {
        try
        {
            await _owerApp.DeactivateAsync(id);

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
    /// Ativa um ower.
    /// </summary>
    /// <param name="id">Identificador do ower a ser ativado.</param>
    /// <returns>
    /// Retorna status 204 quando a ativação é realizada com sucesso.
    /// Retorna status 404 quando o ower não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPatch("{id:int}/activate")]
    public async Task<ActionResult> Activate([FromRoute] int id)
    {
        try
        {
            await _owerApp.ActivateAsync(id);

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