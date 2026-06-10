using Microsoft.AspNetCore.Mvc;
using Application.DTO;
using Microsoft.AspNetCore.Authorization;
[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductApp _productApp;

    /// <summary>
    /// Inicializa uma nova instância do controller de products.
    /// </summary>
    /// <param name="productApp">Serviço de aplicação responsável pelas operações de product.</param>
    public ProductController(IProductApp productApp)
    {
        _productApp = productApp;
    }

    /// <summary>
    /// Adiciona um novo product ao sistema.
    /// </summary>
    /// <param name="request">Dados necessários para criação do product.</param>
    /// <returns>
    /// Retorna status 201 com o identificador do product criado.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Add([FromBody] ProductRequest request)
    {
        try
        {
            var idProduct = await _productApp.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = idProduct }, new { id = idProduct });
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
    /// Obtém um product pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do product.</param>
    /// <returns>
    /// Retorna status 200 com os dados do product encontrado.
    /// Retorna status 404 quando o product não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetById([FromRoute] string id)
    {
        try
        {
            var product = await _productApp.GetByIdAsync(id);

            return Ok(product);
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
    /// Obtém products cadastrados filtrando por status de ativação.
    /// </summary>
    /// <param name="isActive">Filtra por status de ativação (opcional).</param>
    /// <param name="pagina"> Número da página para paginação (padrão: 1).</param>
    /// <param name="quantidadePorPagina">Quantidade de itens por página para paginação (padrão: 10).</param>
    /// <returns>
    /// Retorna status 200 com a coleção de products.
    /// Retorna status 400 quando os parâmetros informados são inválidos.
    /// Retorna status 404 quando nenhum product é localizado.
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
            var products = await _productApp.GetAllAsync(isActive, pagina, quantidadePorPagina);

            return Ok(products);
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
    /// Atualiza os dados cadastrais de um product.
    /// </summary>
    /// <param name="request">Dados atualizados do product.</param>
    /// <returns>
    /// Retorna status 204 quando a atualização é realizada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando o product não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] ProductRequest request)
    {
        try
        {
            await _productApp.UpdateAsync(request);

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
    /// Remove um product do sistema.
    /// </summary>
    /// <param name="id">Identificador do product a ser removido.</param>
    /// <returns>
    /// Retorna status 204 quando a exclusão é realizada com sucesso.
    /// Retorna status 404 quando o product não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete([FromRoute] string id)
    {
        try
        {
            await _productApp.DeleteAsync(id);

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
    /// Desativa um product.
    /// </summary>
    /// <param name="id">Identificador do product a ser desativado.</param>
    /// <returns>
    /// Retorna status 204 quando a desativação é realizada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando o product não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Deactivate([FromRoute] string id)
    {
        try
        {
            await _productApp.DeactivateAsync(id);

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
    /// Ativa um product.
    /// </summary>
    /// <param name="id">Identificador do product a ser ativado.</param>
    /// <returns>
    /// Retorna status 204 quando a ativação é realizada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando o product não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [Authorize]
    [HttpPatch("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Activate([FromRoute] string id)
    {
        try
        {
            await _productApp.ActivateAsync(id);

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
