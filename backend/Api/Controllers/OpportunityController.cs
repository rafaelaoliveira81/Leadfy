using Application.DTO;
using Application.DTOs;
using Domain.Entities;
using Domain.Enuns;
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
            var opportunity = new Opportunity
            {
                LeadId = opportunityRequest.LeadId,
                ProductId = opportunityRequest.ProductId,
                Stage = (OpportunityStage)opportunityRequest.Stage,
                Amount = opportunityRequest.Amount,
                ExpectedCloseDate = opportunityRequest.ExpectedCloseDate
            };

            var idOpportunity = await _opportunityApp.AddAsync(opportunity);

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

            var opportunityResponse = new OpportunityResponse
            {
                ID = opportunity.ID,
                LeadId = opportunity.LeadId,
                LeadName = opportunity.Lead?.Name,
                ProductId = opportunity.ProductId,
                ProductName = opportunity.Product?.Name,
                Stage = (int)opportunity.Stage,
                StageName = opportunity.Stage.ToString(),
                Status = opportunity.IsActive ? "Active" : "Inactive",
                Amount = opportunity.Amount,
                SortOrder = opportunity.SortOrder,
                ExpectedCloseDate = opportunity.ExpectedCloseDate,
                CreatedAt = opportunity.CreatedAt,
                IsActive = opportunity.IsActive
            };

            return Ok(opportunityResponse);
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
    /// <param name="isActive">Filtra por status de ativação (opcional).</param>
    /// <param name="leadId">Filtra por lead específico (opcional).</param>
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
    public async Task<ActionResult> Get([FromQuery] bool? isActive, [FromQuery] int? leadId)
    {
        try
        {
            IEnumerable<Opportunity> opportunities;

            if (isActive.HasValue)
            {
                opportunities = await _opportunityApp.GetAllByStatusAsync(isActive.Value);
            }
            else if (leadId.HasValue && leadId > 0)
            {
                opportunities = await _opportunityApp.GetByLeadIdAsync(leadId.Value);
            }
            else
            {
                opportunities = await _opportunityApp.GetAllAsync();
            }

            var opportunitiesResponse = opportunities.Select(o => new OpportunityResponse
            {
                ID = o.ID,
                LeadId = o.LeadId,
                LeadName = o.Lead?.Name,
                ProductId = o.ProductId,
                ProductName = o.Product?.Name,
                Stage = (int)o.Stage,
                StageName = o.Stage.ToString(),
                Status = o.IsActive ? "Active" : "Inactive",
                Amount = o.Amount,
                SortOrder = o.SortOrder,
                ExpectedCloseDate = o.ExpectedCloseDate,
                CreatedAt = o.CreatedAt,
                IsActive = o.IsActive
            });

            return Ok(opportunitiesResponse);
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
    /// Gera um plano de ação para a opportunity informada usando a configuração de IA escolhida.
    /// </summary>
    /// <param name="id">Identificador da opportunity.</param>
    /// <param name="request">Dados da requisição para geração do plano.</param>
    /// <returns>Plano de ação gerado e persistido.</returns>
    [Authorize]
    [HttpPost("{id:int}/generate-action-plan")]
    [ProducesResponseType(typeof(OpportunityActionPlanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GenerateActionPlan([FromRoute] int id, [FromBody] OpportunityActionPlanGenerate request)
    {
        try
        {
            var dto = await _opportunityActionPlanApp.GenerateAsync(id, request.ConfigId);

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
            var opportunity = new Opportunity
            {
                ID = id,
                LeadId = opportunityRequest.LeadId,
                ProductId = opportunityRequest.ProductId,
                Stage = (OpportunityStage)opportunityRequest.Stage,
                Amount = opportunityRequest.Amount,
                ExpectedCloseDate = opportunityRequest.ExpectedCloseDate,
                IsActive = opportunityRequest.IsActive
            };

            await _opportunityApp.UpdateAsync(opportunity);

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
    /// Desativa uma opportunity.
    /// </summary>
    /// <param name="id">Identificador da opportunity a ser desativada.</param>
    /// <returns>
    /// Retorna status 204 quando a desativação é realizada com sucesso.
    /// Retorna status 404 quando a opportunity não é localizada.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPatch("{id:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Deactivate([FromRoute] int id)
    {
        try
        {
            await _opportunityApp.DeactivateAsync(id);

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
    /// Ativa uma opportunity.
    /// </summary>
    /// <param name="id">Identificador da opportunity a ser ativada.</param>
    /// <returns>
    /// Retorna status 204 quando a ativação é realizada com sucesso.
    /// Retorna status 404 quando a opportunity não é localizada.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPatch("{id:int}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Activate([FromRoute] int id)
    {
        try
        {
            await _opportunityApp.ActivateAsync(id);

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
            await _opportunityApp.ChangeStageAsync(id, (OpportunityStage)request.Stage);

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
            var items = request.Items.Select(i =>
                (i.Id, (OpportunityStage)i.Stage, i.SortOrder));

            await _opportunityApp.UpdateSortOrderAsync(items);

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
