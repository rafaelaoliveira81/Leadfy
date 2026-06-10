---
name: backend-application-validation
description: "Orienta alteracoes na camada Application do backend ASP.NET Core do Leadfy. Use para validacao de request, mapeamento entre DTO e entidade, orquestracao entre repositorios e services, reutilizacao de contratos e padronizacao de ArgumentException e KeyNotFoundException."
argument-hint: "Descreva a regra, validacao, mapeamento ou fluxo da camada Application a ajustar"
user-invocable: true
---

# Backend Application Validation

Use esta skill quando o ponto principal da tarefa estiver na camada `Application`.
Ela serve para organizar validacao de entrada, mapeamento entre DTOs e entidades e orquestracao entre dependencias sem empurrar regra para controller ou repository.

## Papel Desta Skill No Conjunto

Esta skill cobre o slice de regra de aplicacao.
Use-a quando a mudanca dominante estiver em validacao, mapeamento, precondicoes, composicao de resposta ou orquestracao entre repositories e services.
Se a tarefa for mais ampla e cruzar varias camadas sem um centro claro, prefira `backend-feature-flow`.

## Quando Usar

- Criar ou ajustar validacoes de request
- Criar ou ajustar mapeamento entre DTO e entidade
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
4. Separe claramente validacao, mapeamento e persistencia antes de tocar no codigo.

Consulte:

- [Checklist da Application](./references/app-validation-checklist.md)
- [Template de mapeamento DTO](./assets/dto-mapping-template.md)

## Procedimento

1. Valide entradas cedo.

Na Application, valide request, campos obrigatorios, limites, formatos e precondicoes logo no inicio do fluxo.
Quando o dado for invalido, use `ArgumentException` seguindo o padrao textual do modulo.

2. Valide existencia de dependencias de negocio.

Quando a operacao depender de entidades ja persistidas, carregue o dado necessario e falhe cedo com `KeyNotFoundException` se o recurso nao existir.
Concentre esse comportamento em metodos auxiliares previsiveis, como `Validate<Entity>ExistsByIdAsync`.

3. Mapeie DTOs de forma explicita.

Prefira metodos dedicados para transformar request em entidade e entidade em response.
Evite mapeamento espalhado no meio do fluxo principal quando isso reduzir legibilidade ou dificultar manutencao.

4. Orquestre dependencias sem invadir outras camadas.

Use a Application para coordenar mais de um repository ou chamar service quando necessario.
Nao empurre regra de negocio para o controller e nao use repository como ponto de orquestracao.

5. Reuse contratos antes de criar novos.

Antes de criar DTO, interface ou excecao nova, confirme se o modulo ja possui contrato equivalente ou adaptavel.
So crie artefatos novos quando houver necessidade concreta de semantica ou formato.

6. Valide a mudanca.

Se houver uma verificacao estreita para o slice alterado, use-a primeiro.
Caso contrario, rode `dotnet build backend/crm.sln` ou um build mais estreito do projeto afetado.

## Regras Operacionais Deste Projeto

- Controllers devem permanecer finos; a Application e o lugar natural para validacao e orquestracao
- Use `ArgumentException` para entrada invalida e `KeyNotFoundException` para recurso inexistente quando esse ja for o padrao do modulo
- Preserve mensagens em portugues quando o modulo ja usa portugues
- Prefira metodos auxiliares com nomes explicitos para validacao e mapeamento
- Reaproveite DTOs e interfaces existentes antes de criar contratos novos
- Se a transformacao de dados for apenas persistencia, mantenha no Repository; se for montagem de resposta ou regra de fluxo, mantenha na Application

## Resultado Esperado

Ao final, a camada Application deve ficar consistente nestes pontos:

- validacao centralizada e previsivel
- mapeamento explicito entre DTO e entidade
- orquestracao clara entre repositories e services
- excecoes coerentes com o modulo
- controller e repository sem responsabilidades indevidas

## Validacao

1. Confira se o fluxo principal do App continua legivel.
2. Confira se validacoes e mapeamentos nao ficaram espalhados sem necessidade.
3. Execute `dotnet build backend/crm.sln` se nao houver verificacao mais estreita.

## Referencias

- [Checklist da Application](./references/app-validation-checklist.md)
- [Template de mapeamento DTO](./assets/dto-mapping-template.md)
