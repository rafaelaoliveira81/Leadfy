# Checklist Swagger E Contrato HTTP

Use este checklist antes de concluir qualquer alteracao em endpoint.

## Contrato Do Endpoint

- rota correta e coerente com o recurso
- verbo HTTP adequado ao comportamento
- parametros de rota, query e body claramente definidos
- DTO de request e response coerentes com a acao
- `ActionResult` ou `ActionResult<T>` escolhido de forma consistente

## Documentacao XML

- `summary` descreve objetivamente o endpoint
- `param` documenta parametros relevantes da assinatura
- `returns` descreve o resultado esperado em termos de HTTP e payload
- linguagem coerente com o restante do controller, normalmente portugues

## ProducesResponseType

- `200` quando retorna recurso ou colecao com sucesso
- `201` quando cria recurso e devolve localizacao ou identificador
- `204` quando a operacao nao devolve corpo
- `400` quando ha validacao ou argumento invalido
- `401` quando a acao exige autenticacao e esse resultado e possivel
- `404` quando o recurso pode nao existir
- `500` quando o controller trata erro interno generico

Nao documente status code que o codigo nao pode produzir.

## Coerencia Com O Codigo

- cada `return` relevante possui status documentado
- cada `catch` relevante esta refletido na documentacao
- `CreatedAtAction` aponta para uma acao existente e compativel
- `NoContent` nao promete payload no Swagger
- `Ok` devolve o tipo que o contrato anuncia

## Validacao Final

- acao continua fina, sem regra de negocio
- Swagger da acao representa o comportamento real
- build executado apos a alteracao
