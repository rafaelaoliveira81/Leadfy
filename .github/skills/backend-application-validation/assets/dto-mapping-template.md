# Template De Validacao E Mapeamento Na Application

Use este modelo como ponto de partida para organizar um App com validacao, busca de dependencias e mapeamento explicito.

```csharp
public async Task<MeuResponse> ExecuteAsync(MeuRequest request)
{
    ValidateRequest(request);

    var entity = await ValidateEntityExistsByIdAsync(request.Id);

    entity.Name = request.Name;
    entity.Description = request.Description;

    await _repository.UpdateAsync(entity);

    return MapToResponse(entity);
}

private static void ValidateRequest(MeuRequest request)
{
    if (request == null)
        throw new ArgumentException("Request nao pode ser vazia.");

    if (string.IsNullOrWhiteSpace(request.Name))
        throw new ArgumentException("O nome deve ser informado.");
}

private async Task<MinhaEntity> ValidateEntityExistsByIdAsync(int id)
{
    var entity = await _repository.GetByIdAsync(id);

    if (entity == null)
        throw new KeyNotFoundException("Registro nao localizado.");

    return entity;
}

private static MinhaEntity MapToEntity(MeuRequest request)
{
    return new MinhaEntity
    {
        Name = request.Name,
        Description = request.Description
    };
}

private static MeuResponse MapToResponse(MinhaEntity entity)
{
    return new MeuResponse
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description
    };
}
```

## Ajustes Esperados

- adapte os nomes ao modulo real
- use `MapToEntity`, `MapToResponse` ou convencao equivalente do modulo
- remova metodos nao usados pelo fluxo especifico
- mantenha as mensagens coerentes com o padrao textual da feature
- se a operacao for criacao, use `MapToEntity` no inicio do fluxo em vez de carregar entidade existente
