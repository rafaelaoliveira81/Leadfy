using Application;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Api.Models.AiConfig.Request;
using Api.Models.AiConfig.Response;

namespace Api.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de configurações de IA e pela
/// geração de planos de ação para conversão de leads.
/// </summary>
[ApiController]
[Route("api/ai-config")]
public class AiConfigController : ControllerBase
{
    private readonly IAiConfigApp _aiConfigApp;
    private readonly ILeadApp _leadApp;
    private readonly IAiService _aiService;

    /// <summary>
    /// Inicializa uma nova instância do controller de configuração de IA.
    /// </summary>
    public AiConfigController(IAiConfigApp aiConfigApp, ILeadApp leadApp, IAiService aiService)
    {
        _aiConfigApp = aiConfigApp;
        _leadApp = leadApp;
        _aiService = aiService;
    }

    /// <summary>
    /// Cria uma nova configuração de IA.
    /// </summary>
    /// <param name="request">Dados da configuração, incluindo template, modelo e chave de API.</param>
    /// <returns>Configuração criada com a chave mascarada.</returns>
    /// <response code="201">Configuração criada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(AiConfigResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> Add([FromBody] AiConfigSave request)
    {
        try
        {
            var id = await _aiConfigApp.AddAsync(request.PromptTemplate, request.ModelName, request.ApiKey);
            var config = await _aiConfigApp.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id }, MapToResponse(config));
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
    /// Retorna uma configuração de IA pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da configuração.</param>
    /// <returns>Configuração encontrada com a chave mascarada.</returns>
    /// <response code="200">Configuração encontrada.</response>
    /// <response code="404">Configuração não localizada.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AiConfigResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var config = await _aiConfigApp.GetByIdAsync(id);
            return Ok(MapToResponse(config));
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
    /// Retorna a configuração de IA ativa mais recente.
    /// </summary>
    /// <returns>Configuração ativa ou 404 se nenhuma existir.</returns>
    /// <response code="200">Configuração ativa encontrada.</response>
    /// <response code="404">Nenhuma configuração ativa cadastrada.</response>
    [HttpGet("ativa")]
    [ProducesResponseType(typeof(AiConfigResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> GetActive()
    {
        try
        {
            var config = await _aiConfigApp.GetActiveAsync();
            if (config == null)
                return NotFound(new { message = "Nenhuma configuração de IA ativa encontrada." });

            return Ok(MapToResponse(config));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Retorna todas as configurações de IA cadastradas.
    /// </summary>
    /// <returns>Lista de configurações com as chaves mascaradas.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AiConfigResponse>), 200)]
    public async Task<ActionResult> GetAll()
    {
        try
        {
            var configs = await _aiConfigApp.GetAllAsync();
            return Ok(configs.Select(MapToResponse));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza os dados de uma configuração de IA existente.
    /// Quando a chave de API não for informada, a chave atual é mantida.
    /// </summary>
    /// <param name="id">ID da configuração.</param>
    /// <param name="request">Novos dados da configuração.</param>
    /// <response code="204">Atualização realizada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="404">Configuração não localizada.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] AiConfigUpdate request)
    {
        try
        {
            await _aiConfigApp.UpdateAsync(id, request.PromptTemplate, request.ModelName, request.ApiKey);
            return NoContent();
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
    /// Remove fisicamente uma configuração de IA.
    /// </summary>
    /// <param name="id">ID da configuração.</param>
    /// <response code="204">Remoção realizada com sucesso.</response>
    /// <response code="404">Configuração não localizada.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        try
        {
            await _aiConfigApp.DeleteAsync(id);
            return NoContent();
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
    /// Ativa uma configuração de IA.
    /// </summary>
    /// <param name="id">ID da configuração.</param>
    /// <response code="204">Ativação realizada com sucesso.</response>
    /// <response code="404">Configuração não localizada.</response>
    [HttpPatch("{id:int}/activate")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> Activate([FromRoute] int id)
    {
        try
        {
            await _aiConfigApp.ActivateAsync(id);
            return NoContent();
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
    /// Desativa uma configuração de IA.
    /// </summary>
    /// <param name="id">ID da configuração.</param>
    /// <response code="204">Desativação realizada com sucesso.</response>
    /// <response code="404">Configuração não localizada.</response>
    [HttpPatch("{id:int}/deactivate")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> Deactivate([FromRoute] int id)
    {
        try
        {
            await _aiConfigApp.DeactivateAsync(id);
            return NoContent();
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
    /// Gera um plano de ação para conversão do lead utilizando a configuração de IA informada.
    /// </summary>
    /// <param name="configId">ID da configuração de IA a ser utilizada.</param>
    /// <param name="leadId">ID do lead para o qual o plano será gerado.</param>
    /// <returns>Plano de ação gerado pela IA.</returns>
    /// <remarks>
    /// O template da configuração é preenchido com os dados do lead antes do envio ao modelo.
    /// A chave de API é descriptografada internamente e nunca trafega para o cliente.
    /// </remarks>
    /// <response code="200">Plano de ação gerado com sucesso.</response>
    /// <response code="404">Configuração ou lead não localizado.</response>
    [HttpPost("{configId:int}/generate-action-plan/{leadId:int}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> GenerateActionPlan([FromRoute] int configId, [FromRoute] int leadId)
    {
        try
        {
            var config = await _aiConfigApp.GetByIdAsync(configId);
            var lead = await _leadApp.GetByIdAsync(leadId);

            var prompt = _aiConfigApp.BuildPrompt(config, lead);
            var plainApiKey = _aiConfigApp.DecryptApiKey(config.ApiKeyHash);

            var actionPlan = await _aiService.GetResponseFromModel(prompt, config.ModelName, plainApiKey);

            return Ok(new { leadId, configId, actionPlan });
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

    #region Métodos auxiliares

    private static AiConfigResponse MapToResponse(AiConfig config)
    {
        return new AiConfigResponse
        {
            Id = config.Id,
            PromptTemplate = config.PromptTemplate,
            ModelName = config.ModelName,
            ApiKeyMasked = MaskApiKey(config.ApiKeyHash),
            IsActive = config.IsActive,
            Status = config.IsActive ? "Ativo" : "Inativo",
            CreatedAt = config.CreatedAt
        };
    }

    /// <summary>
    /// Mascara a chave criptografada exibindo apenas "****" + últimos 4 chars do valor criptografado.
    /// </summary>
    private static string MaskApiKey(string encryptedKey)
    {
        if (string.IsNullOrWhiteSpace(encryptedKey) || encryptedKey.Length <= 4)
            return "****";

        return $"****{encryptedKey[^4..]}";
    }

    #endregion
}
