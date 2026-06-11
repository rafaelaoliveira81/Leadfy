---
name: backend-endpoint-swagger
description: "Cria ou atualiza endpoints ASP.NET Core com Swagger completo no Leadfy. Use para novas rotas, ajustes de verbo HTTP, ActionResult, autenticacao, contexto de tenantId, XML comments, ProducesResponseType e documentacao OpenAPI em controllers do backend."
argument-hint: "Descreva o endpoint, rota ou contrato HTTP a criar ou ajustar"
user-invocable: true
---

# Backend Endpoint Swagger

Use esta skill quando a tarefa estiver centrada no contrato HTTP de um endpoint do backend.
Ela serve para criar ou ajustar a acao do controller sem deixar a documentacao Swagger divergente do comportamento real.
No contexto atual do projeto, assuma que entidades principais sao tenant-scoped; so entidades explicitamente globais nao dependem de `tenantId`.
No projeto atual, IDs expostos por rota normalmente representam `Guid`; por isso, o contrato HTTP deve deixar isso claro com constraint `:guid` sempre que o endpoint operar por identificador.

## Papel Desta Skill No Conjunto

Esta skill cobre o slice de contrato HTTP e documentacao de endpoint.
Use-a quando o ponto dominante da tarefa estiver no controller, na assinatura da acao, nos status codes ou na documentacao Swagger.
Se a tarefa atravessar varias camadas sem um centro claro, use `backend-feature-flow`.

## Quando Usar

- Criar endpoint novo em controller
- Ajustar rota, verbo HTTP ou assinatura de uma acao existente
- Ajustar endpoint autenticado que depende do tenant do usuario logado
- Corrigir ou completar XML comments
- Corrigir ou completar `ProducesResponseType`
- Ajustar resposta HTTP, autenticacao ou tipo de retorno de uma acao

## Nao Use Quando

- A tarefa exigir uma feature completa atraves de varias camadas
- A mudanca estiver restrita a repositorio, Dapper ou EF Core
- O foco principal for JWT, claims ou configuracao de autenticacao
- O ponto dominante da mudanca estiver em validacao de Application, e nao no contrato HTTP

Nesses casos, prefira uma skill mais ampla ou mais especializada.

## Antes De Editar

1. Leia o controller alvo e uma acao vizinha com estilo semelhante.
2. Confirme o DTO de entrada e o payload esperado na resposta.
3. Verifique se a acao exige `Authorize`.
4. Confirme se o endpoint e tenant-scoped ou uma excecao global.
5. Mapeie os status codes realmente possiveis a partir de `return`, `catch` e excecoes esperadas.
6. Confirme se a mudanca exige atualizacao de contrato em App ou DTO, mesmo que a edicao principal esteja no controller.

Consulte:

- [Checklist Swagger](./references/swagger-checklist.md)
- [Template de controller](./assets/controller-doc-template.md)

## Procedimento

1. Defina o contrato HTTP antes da implementacao.

Confirme rota, verbo, parametros de rota ou query, request body, formato da resposta, necessidade de autenticacao e de onde vem o contexto de tenant.
Quando houver identificador de entidade na rota, siga o padrao `{id:guid}` ou nome equivalente com `:guid`, mesmo que o parametro da acao permaneça como `string` por compatibilidade com o modulo.

2. Escolha a assinatura adequada da acao.

Prefira `ActionResult` ou `ActionResult<T>` quando isso deixar o contrato mais claro no Swagger.
Mantenha o estilo ja dominante no controller alvo.

3. Implemente ou ajuste a acao mantendo o controller fino.

O controller deve receber a requisicao, delegar para a camada Application e traduzir o resultado para HTTP.
Nao mova regra de negocio para a camada Api apenas para simplificar a acao.

4. Sincronize a documentacao da acao com o codigo.

Adicione ou ajuste `summary`, `param`, `returns` e `ProducesResponseType` para refletir os retornos reais da acao.
Se o metodo usa `CreatedAtAction`, `NoContent`, `Ok`, `BadRequest`, `NotFound` ou `StatusCode(500)`, a documentacao deve acompanhar exatamente esses resultados.

5. Revise autenticacao e coerencia do endpoint.

Se a rota depende de usuario autenticado, preserve `Authorize` e nao altere o fluxo de claims sem necessidade.
Em endpoints tenant-scoped, o `tenantId` deve vir do contexto autenticado ou de abstração equivalente, nao de rota, query ou body arbitrarios, salvo um caso global ou administrativo explicitamente definido.
Se a acao receber um `id` textual, documente e implemente o fluxo assumindo que esse valor representa um `Guid` valido e que sera validado cedo pela camada seguinte.
Garanta coerencia entre rota, DTO, codigos de resposta e tipo retornado.

6. Valide a mudanca.

Se houver uma verificacao mais estreita para o slice alterado, prefira essa validacao.
Caso contrario, rode `dotnet build backend/crm.sln` ou um build mais estreito do projeto afetado.

## Regras Operacionais Deste Projeto

- Preserve o estilo ja usado em `backend/Api/Controllers`
- Escreva documentacao em portugues quando o controller ja segue esse padrao
- Controllers devem permanecer finos e orientados a HTTP
- Endpoints tenant-scoped devem derivar `tenantId` do contexto autenticado, nao expor esse acoplamento como entrada livre sem necessidade
- Endpoints por identificador devem usar `:guid` na rota quando o recurso e identificado por `Guid`
- Um parametro `string id` em controller deve ser entendido como transporte de um `Guid`, nao como identificador arbitrario
- `ProducesResponseType` deve refletir o comportamento real da acao
- XML comments devem documentar parametros relevantes e retorno esperado
- Nao altere Swagger global, CORS ou JWT em `Program.cs` sem necessidade real

## Resultado Esperado

Ao final, o endpoint deve ficar consistente nestes pontos:

- rota, verbo e assinatura coerentes
- autenticacao alinhada ao caso de uso
- contexto de tenant coerente com o modelo autenticado quando a entidade nao for global
- retorno HTTP claro para consumidor e Swagger
- XML comments atualizados
- `ProducesResponseType` sincronizado com o codigo

## Validacao

1. Revise se cada status code documentado realmente pode acontecer.
2. Revise se o tipo de retorno condiz com o payload devolvido.
3. Revise se endpoints tenant-scoped nao recebem `tenantId` livremente quando o backend ja o resolve por contexto autenticado.
4. Revise se endpoints por identificador usam constraint `:guid` e mantem a semantica de `Guid` no contrato.
5. Execute `dotnet build backend/crm.sln` se nao houver validacao mais estreita.

## Referencias

- [Checklist Swagger](./references/swagger-checklist.md)
- [Template de controller](./assets/controller-doc-template.md)
