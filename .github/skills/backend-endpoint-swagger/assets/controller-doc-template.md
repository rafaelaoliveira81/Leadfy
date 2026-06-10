# Template De Endpoint Documentado

Use este modelo como ponto de partida para novas acoes em controller.

```csharp
/// <summary>
/// Descreve objetivamente o que o endpoint faz.
/// </summary>
/// <param name="parametro">Descreve o parametro quando ele fizer parte da assinatura.</param>
/// <returns>
/// Retorna status 200 com o payload esperado.
/// Retorna status 400 quando os dados informados sao invalidos.
/// Retorna status 404 quando o recurso nao e localizado.
/// Retorna status 500 em caso de erro interno.
/// </returns>
[Authorize]
[HttpGet("{id:int}")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public async Task<ActionResult<MeuResponse>> GetById([FromRoute] int id)
{
    try
    {
        var response = await _app.GetByIdAsync(id);

        return Ok(response);
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
```

## Ajustes Esperados

- troque verbo e rota conforme o caso
- remova `Authorize` quando o endpoint nao exigir autenticacao
- ajuste os status codes para refletir exatamente o codigo real
- troque `MeuResponse` pelo DTO real
- nao mantenha `400`, `404` ou `500` se a acao nao puder retornar esses codigos
