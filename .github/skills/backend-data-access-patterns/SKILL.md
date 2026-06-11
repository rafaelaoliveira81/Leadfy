---
name: backend-data-access-patterns
description: "Orienta mudancas de acesso a dados no backend ASP.NET Core do Leadfy. Use para criar ou ajustar metodos de Repository, aplicar filtros por tenantId em entidades tenant-scoped, decidir entre EF Core e Dapper, trabalhar com stored procedures, corrigir problemas de conexao, mapping e assinatura de parametros."
argument-hint: "Descreva o metodo de repositorio, consulta, procedure ou problema de acesso a dados"
user-invocable: true
---

# Backend Data Access Patterns

Use esta skill quando a tarefa estiver centrada na camada Repository ou na forma como o backend acessa dados.
Ela serve para decidir entre EF Core e Dapper com base no padrao real da feature, mantendo persistencia isolada e evitando regressao em stored procedures e conexoes.
No contexto atual do projeto, assuma que entidades principais sao tenant-scoped e exigem `tenantId`; apenas entidades explicitamente globais nao entram nesse filtro.
No projeto atual, identificadores persistidos sao `Guid`; a camada Repository deve trabalhar com tipos `Guid` ou `Guid?`, e nao com texto cru vindo de HTTP.

## Papel Desta Skill No Conjunto

Esta skill cobre o slice de persistencia.
Use-a quando a mudanca dominante estiver em metodo de repository, query, procedure, mapeamento de retorno bruto ou decisao entre EF Core e Dapper.
Se a alteracao exigir coordenacao ampla entre controller, Application e Repository, prefira `backend-feature-flow`.

## Quando Usar

- Criar novo metodo de repositorio
- Ajustar um metodo existente de Repository
- Ajustar filtro, escrita ou consulta considerando `tenantId`
- Decidir entre EF Core e Dapper para uma feature
- Corrigir parametros ou assinatura de stored procedure
- Investigar problema de conexao, query ou mapeamento no repositorio
- Revisar se uma alteracao de persistencia invadiu regra de negocio

## Nao Use Quando

- A mudanca principal estiver no contrato HTTP do endpoint
- A tarefa for uma feature completa entre varias camadas
- O foco principal for validacao de Application ou JWT
- O problema principal for apenas documentar ou proteger endpoint

Nesses casos, prefira uma skill mais aderente ao ponto de controle da mudanca.

## Antes De Editar

1. Localize um repositorio ou metodo equivalente na mesma feature.
2. Confirme se o modulo atual ja usa EF Core, Dapper ou ambos.
3. Descubra se existe stored procedure, funcao SQL ou entidade EF ja usada nesse fluxo.
4. Classifique a entidade como tenant-scoped ou global antes de definir filtros e parametros.
5. Separe o que e persistencia do que e regra de negocio antes de editar.

Consulte:

- [EF Core vs Dapper](./references/ef-vs-dapper.md)
- [Checklist de repository](./references/repository-checklist.md)

## Procedimento

1. Inspecione o padrao existente antes de escolher a tecnologia de acesso.

Se a feature ja usa EF Core para consultas relacionais e agregacao simples, mantenha EF Core.
Se o fluxo ja depende de stored procedures, funcoes SQL ou comandos Dapper, preserve esse padrao, salvo motivo concreto para mudar.

2. Mantenha o repositorio focado em persistencia.

No repository, implemente leitura, escrita e mapeamento de dados.
Nao mova validacao de negocio, decisao de fluxo ou traducao HTTP para essa camada.
Quando a camada superior ainda receber IDs como `string`, faca a conversao antes de entrar no repository; nao transforme o repository em ponto de parse de request.

3. Ao usar Dapper, trate conexao e parametros com rigor.

Crie uma nova conexao por chamada usando o mecanismo do projeto.
Nao reutilize ou descarte diretamente `Database.GetDbConnection()` do `DbContext` para esses metodos.
Monte o objeto de parametros de forma estritamente alinhada a assinatura real da stored procedure.
Se a entidade for tenant-scoped, inclua `tenantId` nos parametros, filtros e comandos; so omita esse dado em fluxos explicitamente globais.
Quando o parametro representar identificador, envie `Guid` tipado para a query ou procedure sempre que a assinatura do modulo seguir esse padrao.

4. Ao usar EF Core, siga o estilo do modulo.

Use `Include`, filtros, ordenacao e materializacao no mesmo padrao da feature vizinha.
Preserve query filters, comparacoes e atribuicoes de `tenantId` quando a entidade for tenant-scoped.
Evite misturar regra de negocio com a query apenas porque o LINQ permite.

5. Revise impacto de mapeamento e retorno.

Confirme se o metodo retorna entidade, DTO bruto ou colecao no mesmo estilo da interface correspondente.
Se houver transformacao de dados mais rica, empurre isso para `Application`.

6. Valide a mudanca.

Se houver build estreito do projeto afetado, use-o.
Caso contrario, rode `dotnet build backend/crm.sln`.

## Regras Operacionais Deste Projeto

- O projeto usa EF Core e Dapper; escolha com base no padrao existente da feature
- Repositorios devem permanecer focados em persistencia
- Entidades principais devem ser filtradas por `tenantId`; apenas entidades explicitamente globais podem ignorar esse contexto
- Identificadores de entidade devem chegar ao repository como `Guid` ou `Guid?`, nao como `string` de contrato HTTP
- Metodos Dapper devem criar nova conexao por chamada
- Stored procedures exigem parametros exatamente alinhados a assinatura real
- Nao contorne filtros de tenant em consultas ou updates sem uma necessidade global explicitamente definida
- Quando o repositório devolver dado bruto para transformacao posterior, a desserializacao ou montagem mais rica deve ficar fora do Repository quando esse ja for o padrao do modulo
- Evite alterar interfaces, DTOs ou Application sem necessidade; se isso for inevitavel, trate a tarefa como mudanca multi-camada

## Resultado Esperado

Ao final, a alteracao de acesso a dados deve ficar consistente nestes pontos:

- tecnologia de acesso escolhida com base no padrao real do modulo
- filtros e persistencia coerentes com o `tenantId` quando a entidade nao for global
- repository sem regra de negocio
- conexao Dapper segura e isolada por chamada
- parametros e mapeamento coerentes com banco e interface
- validacao executada apos a edicao

## Validacao

1. Confira se o metodo segue o mesmo estilo do repository vizinho.
2. Confira se parametros de stored procedure estao completos e sem extras.
3. Confira se consultas e writes tenant-scoped preservam o filtro por `tenantId`.
4. Confira se nenhum metodo novo do repository passou a aceitar `string id` quando o modulo usa `Guid`.
5. Execute `dotnet build backend/crm.sln` se nao houver uma verificacao mais estreita.

## Referencias

- [EF Core vs Dapper](./references/ef-vs-dapper.md)
- [Checklist de repository](./references/repository-checklist.md)
