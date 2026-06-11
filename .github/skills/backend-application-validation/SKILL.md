---
name: backend-application-validation
description: "Orienta alteracoes na camada Application do backend ASP.NET Core do Leadfy. Use para validacao de request, mapeamento entre DTO e entidade, propagacao de tenantId em entidades tenant-scoped, orquestracao entre repositorios e services, reutilizacao de contratos e padronizacao de ArgumentException e KeyNotFoundException."
argument-hint: "Descreva a regra, validacao, mapeamento ou fluxo da camada Application a ajustar"
user-invocable: true
---

# Backend Application Validation

Use esta skill quando o ponto principal da tarefa estiver na camada `Application`.
Ela serve para organizar validacao de entrada, mapeamento entre DTOs e entidades e orquestracao entre dependencias sem empurrar regra para controller ou repository.
No contexto atual do projeto, trate entidades principais como tenant-scoped por padrao; apenas entidades explicitamente globais nao devem receber ou validar `tenantId`.
No projeto atual, IDs de entidades sao `Guid`; a Application costuma receber esses valores como `string` em requests e parametros externos, mas precisa valida-los e convertelos cedo.

## Papel Desta Skill No Conjunto

Esta skill cobre o slice de regra de aplicacao.
Use-a quando a mudanca dominante estiver em validacao, mapeamento, precondicoes, composicao de resposta ou orquestracao entre repositories e services.
Se a tarefa for mais ampla e cruzar varias camadas sem um centro claro, prefira `backend-feature-flow`.

## Quando Usar

- Criar ou ajustar validacoes de request
- Criar ou ajustar mapeamento entre DTO e entidade
- Propagar `tenantId` corretamente em operacoes tenant-scoped
- Orquestrar mais de um repository ou service na mesma operacao
- Padronizar excecoes de negocio na Application
- Reaproveitar ou reorganizar DTOs e interfaces existentes

## Nao Use Quando

- A tarefa principal for contrato HTTP e documentacao Swagger
- A mudanca estiver isolada em Repository ou acesso a dados
- O foco principal for JWT, claims ou autenticacao
- A entrega principal for montar uma feature completa em varias camadas

Nesses casos, prefira a skill mais aderente ao ponto de controle do comportamento.

## Antes De Editar

1. Leia um App equivalente no mesmo modulo antes de propor nova estrutura.
2. Identifique onde a regra deve viver: validacao simples na Application ou regra de dominio mais reutilizavel em Service.
3. Confirme quais DTOs, interfaces e repositories ja existem e podem ser reaproveitados.
4. Classifique a entidade do fluxo como tenant-scoped ou global antes de tocar no codigo.
5. Separe claramente validacao, mapeamento e persistencia antes de tocar no codigo.

Consulte:

- [Checklist da Application](./references/app-validation-checklist.md)
- [Template de mapeamento DTO](./assets/dto-mapping-template.md)

## Procedimento

1. Valide entradas cedo.

Na Application, valide request, campos obrigatorios, limites, formatos e precondicoes logo no inicio do fluxo.
Quando o dado for invalido, use `ArgumentException` seguindo o padrao textual do modulo.
Quando o fluxo receber identificadores textuais, valide com `Guid.TryParse` logo no inicio e nao deixe `string` representando ID seguir adiante sem conversao.

2. Valide existencia de dependencias de negocio.

Quando a operacao depender de entidades ja persistidas, carregue o dado necessario e falhe cedo com `KeyNotFoundException` se o recurso nao existir.
Concentre esse comportamento em metodos auxiliares previsiveis, como `Validate<Entity>ExistsByIdAsync`.

3. Resolva o contexto de tenant cedo quando a entidade nao for global.

Se a operacao for tenant-scoped, obtenha o `tenantId` a partir do contexto autenticado, normalmente via `ITenantProvider`, e use esse valor para montar ou validar a entidade.
Nao confie em `tenantId` vindo livremente do request quando o fluxo pertence ao tenant autenticado.
Se o `tenantId` vier por helper como texto, trate-o como representacao de `Guid` ja validada; se a origem nao garantir isso, faca a validacao antes do uso.

4. Mapeie DTOs de forma explicita.

Prefira metodos dedicados para transformar request em entidade e entidade em response.
Evite mapeamento espalhado no meio do fluxo principal quando isso reduzir legibilidade ou dificultar manutencao.
Ao montar entidades, converta IDs textuais para `Guid` ou `Guid?`; ao montar responses, converta de volta para `string` apenas quando esse for o contrato atual do DTO.

5. Orquestre dependencias sem invadir outras camadas.

Use a Application para coordenar mais de um repository ou chamar service quando necessario.
Nao empurre regra de negocio para o controller e nao use repository como ponto de orquestracao.

6. Reuse contratos antes de criar novos.

Antes de criar DTO, interface ou excecao nova, confirme se o modulo ja possui contrato equivalente ou adaptavel.
So crie artefatos novos quando houver necessidade concreta de semantica ou formato.

7. Valide a mudanca.

Se houver uma verificacao estreita para o slice alterado, use-a primeiro.
Caso contrario, rode `dotnet build backend/crm.sln` ou um build mais estreito do projeto afetado.

## Regras Operacionais Deste Projeto

- Controllers devem permanecer finos; a Application e o lugar natural para validacao e orquestracao
- Use `ArgumentException` para entrada invalida e `KeyNotFoundException` para recurso inexistente quando esse ja for o padrao do modulo
- Em entidades tenant-scoped, resolva `tenantId` pelo contexto autenticado e nao por entrada arbitraria do cliente
- So trate uma entidade sem `tenantId` como valida quando ela for explicitamente global
- Trate todo identificador recebido de fora como `Guid` semantico, mesmo quando o tipo de transporte for `string`
- Antes de chamar repository, converta IDs textuais para `Guid` e rejeite `Guid.Empty` quando o fluxo exigir identificador preenchido
- Preserve mensagens em portugues quando o modulo ja usa portugues
- Prefira metodos auxiliares com nomes explicitos para validacao e mapeamento
- Reaproveite DTOs e interfaces existentes antes de criar contratos novos
- Se a transformacao de dados for apenas persistencia, mantenha no Repository; se for montagem de resposta ou regra de fluxo, mantenha na Application

## Resultado Esperado

Ao final, a camada Application deve ficar consistente nestes pontos:

- validacao centralizada e previsivel
- contexto de tenant coerente nas operacoes tenant-scoped
- mapeamento explicito entre DTO e entidade
- orquestracao clara entre repositories e services
- excecoes coerentes com o modulo
- controller e repository sem responsabilidades indevidas

## Validacao

1. Confira se o fluxo principal do App continua legivel.
2. Confira se validacoes e mapeamentos nao ficaram espalhados sem necessidade.
3. Confira se o `tenantId` nao esta vindo do request em fluxos que deveriam usar o tenant autenticado.
4. Confira se IDs textuais sao validados com `Guid.TryParse` antes de uso em repositories e entidades.
5. Execute `dotnet build backend/crm.sln` se nao houver verificacao mais estreita.

## Referencias

- [Checklist da Application](./references/app-validation-checklist.md)
- [Template de mapeamento DTO](./assets/dto-mapping-template.md)
