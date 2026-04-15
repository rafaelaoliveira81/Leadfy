using Microsoft.AspNetCore.Mvc;
using Models.Request;
using Models.Response;
using Domain.Entities;

namespace Api.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de gerenciamento de leads.
/// </summary>
[ApiController]
[Route("leads")]
public class LeadController : ControllerBase
{
    private readonly ILeadApp _leadApp;

    /// <summary>
    /// Inicializa uma nova instância do controller de leads.
    /// </summary>
    /// <param name="leadApp">Serviço de aplicação responsável pelas operações de lead.</param>
    public LeadController(ILeadApp leadApp)
    {
        _leadApp = leadApp;
    }

    /// <summary>
    /// Adiciona um novo lead ao sistema.
    /// </summary>
    /// <param name="leadRequest">Dados necessários para criação do lead.</param>
    /// <returns>
    /// Retorna status 201 com o identificador do lead criado.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Add([FromBody] LeadAdd leadRequest)
    {
        try
        {
            var lead = new Lead
            {
                Name = leadRequest.Name,
                Email = leadRequest.Email,
                PhoneNumber = leadRequest.PhoneNumber
            };

            var idLead = await _leadApp.AddAsync(lead);

            return CreatedAtAction(nameof(GetById), new { id = idLead }, new { id = idLead });
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
    /// Obtém um lead pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do lead.</param>
    /// <returns>
    /// Retorna status 200 com os dados do lead encontrado.
    /// Retorna status 404 quando o lead não é localizado.
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
            var lead = await _leadApp.GetByIdAsync(id);

            var leadResponse = new LeadResponse
            {
                ID = lead.Id,
                Name = lead.Name,
                Email = lead.Email,
                PhoneNumber = lead.PhoneNumber,
                IsActive = lead.IsActive,
                CreatedAt = lead.CreatedAt
            };

            return Ok(leadResponse);
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
    /// Obtém leads cadastrados. Permite filtrar por status de ativação ou nome.
    /// </summary>
    /// <param name="isActive">Filtra por status de ativação (opcional).</param>
    /// <param name="name">Filtra por nome contendo o valor (opcional).</param>
    /// <returns>
    /// Retorna status 200 com a coleção de leads.
    /// Retorna status 400 quando os parâmetros informados são inválidos.
    /// Retorna status 404 quando nenhum lead é localizado na busca por nome.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Get([FromQuery] bool? isActive, [FromQuery] string name)
    {
        try
        {
            IEnumerable<Lead> leads;

            if (isActive.HasValue)
            {
                leads = await _leadApp.GetAllByStatusAsync(isActive.Value);
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                leads = await _leadApp.GetByNameContainingAsync(name);
            }
            else
            {
                leads = await _leadApp.GetAllAsync();
            }

            var leadsResponse = leads.Select(l => new LeadResponse
            {
                ID = l.Id,
                Name = l.Name,
                Email = l.Email,
                PhoneNumber = l.PhoneNumber,
                IsActive = l.IsActive,
                CreatedAt = l.CreatedAt
            });

            return Ok(leadsResponse);
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
    /// Atualiza os dados cadastrais de um lead.
    /// </summary>
    /// <param name="id">Identificador do lead a ser atualizado.</param>
    /// <param name="leadRequest">Dados atualizados do lead.</param>
    /// <returns>
    /// Retorna status 204 quando a atualização é realizada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando o lead não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] LeadUpdate leadRequest)
    {
        try
        {
            var lead = new Lead
            {
                Id = id,
                Name = leadRequest.Name,
                Email = leadRequest.Email,
                PhoneNumber = leadRequest.PhoneNumber
            };

            await _leadApp.UpdateAsync(lead);

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
    /// Remove um lead do sistema.
    /// </summary>
    /// <param name="id">Identificador do lead a ser removido.</param>
    /// <returns>
    /// Retorna status 204 quando a exclusão é realizada com sucesso.
    /// Retorna status 404 quando o lead não é localizado.
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
            await _leadApp.DeleteAsync(id);

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
    /// Desativa um lead.
    /// </summary>
    /// <param name="id">Identificador do lead a ser desativado.</param>
    /// <returns>
    /// Retorna status 204 quando a desativação é realizada com sucesso.
    /// Retorna status 404 quando o lead não é localizado.
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
            await _leadApp.DeactivateAsync(id);

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
    /// Ativa um lead.
    /// </summary>
    /// <param name="id">Identificador do lead a ser ativado.</param>
    /// <returns>
    /// Retorna status 204 quando a ativação é realizada com sucesso.
    /// Retorna status 404 quando o lead não é localizado.
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
            await _leadApp.ActivateAsync(id);

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
