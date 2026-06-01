using Application.DTO;
using Application.DTOs;
using Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller responsável pelos endpoints de gerenciamento de opportunities.
/// </summary>
[ApiController]
[Route("api/opportunities")]
public class OpportunityController : ControllerBase
{
    private readonly IOpportunityApp _opportunityApp;
    private readonly IOpportunityActionPlanApp _opportunityActionPlanApp;

    /// <summary>
    /// Inicializa uma nova instância do controller de opportunities.
    /// </summary>
    /// <param name="opportunityApp">Serviço de aplicação responsável pelas operações de opportunity.</param>
    /// <param name="opportunityActionPlanApp">Serviço de aplicação responsável pela geração do plano de ação.</param>
    public OpportunityController(IOpportunityApp opportunityApp, IOpportunityActionPlanApp opportunityActionPlanApp)
    {
        _opportunityApp = opportunityApp;
        _opportunityActionPlanApp = opportunityActionPlanApp;
    }

    /// <summary>
    /// Adiciona uma nova opportunity ao sistema.
    /// </summary>
    /// <param name="opportunityRequest">Dados necessários para criação da opportunity.</param>
    /// <returns>
    /// Retorna status 201 com o identificador da opportunity criada.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Add([FromBody] OpportunityAdd opportunityRequest)
    {
        try
        {
            var userId = User.GetAuthenticatedUserId();

            var idOpportunity = await _opportunityApp.AddAsync(opportunityRequest, userId);

            return CreatedAtAction(nameof(GetById), new { id = idOpportunity }, new { id = idOpportunity });
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
    /// Obtém uma opportunity pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da opportunity.</param>
    /// <returns>
    /// Retorna status 200 com os dados da opportunity encontrada.
    /// Retorna status 404 quando a opportunity não é localizada.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var opportunity = await _opportunityApp.GetByIdAsync(id);

            return Ok(opportunity);
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
    /// Obtém opportunities cadastradas. Permite filtrar por status ou lead.
    /// </summary>
    /// <returns>
    /// Retorna status 200 com a coleção de opportunities.
    /// Retorna status 400 quando os parâmetros informados são inválidos.
    /// Retorna status 404 quando nenhuma opportunity é localizada na busca.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Get()
    {
        try
        {
            var opportunities = await _opportunityApp.GetAllAsync();

            return Ok(opportunities);
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
    /// Obtém opportunities de uma stage específica.
    /// </summary>
    /// <param name="stage">Stage da opportunity.</param>
    /// <returns>
    /// Retorna status 200 com a coleção de opportunities da stage informada.
    /// Retorna status 400 quando o stage informado é inválido.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpGet("stage/{stage:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetByStage([FromRoute] int stage)
    {
        try
        {
            var opportunities = await _opportunityApp.GetByStageAsync(stage);

            return Ok(opportunities);
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
    /// Gera um plano de ação para a opportunity informada usando o prompt de IA escolhido.
    /// </summary>
    /// <param name="id">Identificador da opportunity.</param>
    /// <param name="request">Dados da requisição contendo o identificador do prompt de IA.</param>
    /// <returns>Plano de ação gerado e persistido.</returns>
    [Authorize]
    [HttpPost("{id:int}/generate-action-plan")]
    [ProducesResponseType(typeof(OpportunityActionPlanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GenerateActionPlan([FromRoute] int id, [FromBody] GenerateActionPlanRequest request)
    {
        try
        {
            var dto = await _opportunityActionPlanApp.GenerateAsync(id, request.PromptId);

            return Ok(dto);
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
    /// Obtém todos os planos de ação gerados para uma opportunity.
    /// </summary>
    /// <param name="id">Identificador da opportunity.</param>
    /// <returns>Lista de planos de ação ordenada por data de geração decrescente.</returns>
    [Authorize]
    [HttpGet("{id:int}/action-plans")]
    [ProducesResponseType(typeof(IEnumerable<OpportunityActionPlanDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetActionPlans([FromRoute] int id)
    {
        try
        {
            var dtos = await _opportunityActionPlanApp.GetByOpportunityIdAsync(id);

            return Ok(dtos);
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
    /// Atualiza os dados cadastrais de uma opportunity.
    /// </summary>
    /// <param name="id">Identificador da opportunity a ser atualizada.</param>
    /// <param name="opportunityRequest">Dados atualizados da opportunity.</param>
    /// <returns>
    /// Retorna status 204 quando a atualização é realizada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando a opportunity não é localizada.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] OpportunityUpdate opportunityRequest)
    {
        try
        {
            await _opportunityApp.UpdateAsync(id, opportunityRequest);

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
    /// Remove uma opportunity do sistema.
    /// </summary>
    /// <param name="id">Identificador da opportunity a ser removida.</param>
    /// <returns>
    /// Retorna status 204 quando a exclusão é realizada com sucesso.
    /// Retorna status 404 quando a opportunity não é localizada.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        try
        {
            await _opportunityApp.DeleteAsync(id);

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
    /// Altera o stage de uma opportunity (usado pelo Kanban).
    /// </summary>
    /// <param name="id">Identificador da opportunity.</param>
    /// <param name="request">Novo stage.</param>
    [Authorize]
    [HttpPatch("{id:int}/stage")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateStage([FromRoute] int id, [FromBody] OpportunityStageUpdate request)
    {
        try
        {
            await _opportunityApp.ChangeStageAsync(id, request.Stage);

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
    /// Atualiza o stage e a ordenação de múltiplas opportunities em lote (usado pelo Kanban).
    /// </summary>
    /// <param name="request">Lista de itens com id, stage e sortOrder.</param>
    [Authorize]
    [HttpPatch("reorder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Reorder([FromBody] OpportunitySortOrderUpdate request)
    {
        try
        {
            await _opportunityApp.UpdateSortOrderAsync(request.Items);

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
