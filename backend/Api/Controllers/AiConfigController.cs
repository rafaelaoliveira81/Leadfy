using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Application.DTO;
using Application.Extensions;
using Dominio.Enums;

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

    /// <summary>
    /// Inicializa uma nova instância do controller de configuração de IA.
    /// </summary>
    public AiConfigController(IAiConfigApp aiConfigApp)
    {
        _aiConfigApp = aiConfigApp;
    }

    /// <summary>
    /// Retorna a lista de modelos de IA disponíveis.
    /// </summary>
    /// <returns>Lista de modelos com id e label.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet("models")]
    [ProducesResponseType(200)]
    public ActionResult GetModels()
    {
        var models = Enum.GetValues<AiModelsEnum>()
            .Select(m => new { id = (int)m, label = m.GetDescription() });
        return Ok(models);
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
    public async Task<ActionResult> Add([FromBody] AiConfigRequest request)
    {
        try
        {
            var id = await _aiConfigApp.AddAsync(request);
            return Ok(id);
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
    /// Retorna a configuração de IA ativa.
    /// </summary>
    /// <returns>Configuração ativa com a chave mascarada.</returns>
    /// <response code="200">Configuração ativa encontrada.</response>
    /// <response code="404">Nenhuma configuração ativa localizada.</response>
    [HttpGet("ativa")]
    [ProducesResponseType(typeof(AiConfigResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> GetActive()
    {
        try
        {
            var config = await _aiConfigApp.GetActiveAsync();
            return Ok(MapToResponse(config));
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
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] AiConfigRequest request)
    {
        try
        {
            await _aiConfigApp.UpdateAsync(id, request);
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

    #region Métodos auxiliares

    private static AiConfigResponse MapToResponse(AiConfig config)
    {
        return new AiConfigResponse
        {
            Id = config.Id,
            Title = config.Title,
            PromptTemplate = config.PromptTemplate,
            Model = (int)config.Model,
            ApiKeyMasked = MaskApiKey(config.ApiKeyHash),
            IsActive = config.IsActive,
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
