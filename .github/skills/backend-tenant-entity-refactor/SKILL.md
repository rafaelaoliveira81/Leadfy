---
name: backend-tenant-entity-refactor
description: "Orienta a refatoracao de entidades do backend ASP.NET Core do Leadfy para inclusao de tenantId. Use para adicionar TenantId em entidades tenant-scoped, ajustar Entity Framework Core, migrations, repositories, DTOs, Application e isolamento por tenant sem afetar entidades globais."
argument-hint: "Descreva a entidade ou conjunto de entidades que precisa receber tenantId"
user-invocable: true
---

# Backend Tenant Entity Refactor

Use esta skill quando a tarefa estiver centrada em refatorar entidades existentes do backend para inclusao de `tenantId`.
Ela serve para conduzir a mudanca de forma coordenada entre `Domain`, `Repository`, `Application` e banco de dados, sem tratar `tenantId` como um simples campo isolado.
No contexto atual do projeto, entidades principais devem ser tenant-scoped por padrao; apenas entidades explicitamente globais devem permanecer sem `tenantId`.
No projeto atual, `tenantId` e identificadores de entidades devem ser `Guid`; nao use `string` como tipo de dominio para essa informacao.

## Papel Desta Skill No Conjunto

Esta skill cobre o slice de refatoracao estrutural para multi-tenant.
Use-a quando a mudanca dominante estiver em introduzir `tenantId` em entidade, configuracao EF Core, filtros, migration, repositorio, DTO e fluxo de aplicacao.
Se a tarefa for uma feature completa com novo endpoint ou caso de uso mais amplo, prefira `backend-feature-flow`.
Se o foco principal estiver apenas em Repository, prefira `backend-data-access-patterns`.

## Quando Usar

- Adicionar `TenantId` a uma entidade existente
- Refatorar varias entidades principais para isolamento por tenant
- Ajustar configuracoes EF Core para relacionamento com `Tenant`
- Atualizar indexes, constraints e unicidade para considerar `tenantId`
- Corrigir repositories, queries Dapper ou stored procedures para respeitar tenant
- Criar migration para introduzir `tenantId` em tabelas ja existentes
- Revisar DTOs e Application para parar de aceitar `tenantId` arbitrario do cliente

## Nao Use Quando

- A mudanca for apenas em claims, JWT ou autenticacao
- A tarefa estiver restrita ao contrato HTTP de um endpoint
- O ajuste estiver concentrado apenas em uma query isolada de repository
- A entidade for global e a tarefa nao envolver decisao de tenancy

Nesses casos, prefira a skill especializada no ponto dominante da mudanca.

## Antes De Editar

1. Classifique cada entidade alvo como tenant-scoped ou global.
2. Localize o conjunto minimo de arquivos afetados: entidade, configuracao EF Core, `CRMContext`, repositories, DTOs, Application, procedures e migration.
3. Identifique como os dados existentes serao tratados quando a tabela ja possuir registros sem `tenantId`.
4. Verifique se a unicidade atual da entidade precisa virar unicidade por tenant.
5. Leia uma entidade ja tenant-scoped do projeto para copiar o padrao de `Guid`, indice, foreign key e filtro.

Consulte:

- [Checklist de classificacao e impacto](./references/tenant-classification-checklist.md)
- [Notas de migration e backfill](./references/migration-backfill-notes.md)
- [Template de refatoracao tenant-scoped](./assets/tenant-refactor-template.md)

## Procedimento

1. Defina o escopo real da refatoracao.

Confirme quais entidades sao principais e precisam receber `tenantId`.
Se alguma entidade for global, mantenha essa excecao documentada no proprio raciocinio da mudanca e nao espalhe `tenantId` sem necessidade.

2. Atualize a entidade de dominio primeiro.

Adicione `TenantId` como `Guid` na entidade tenant-scoped.
Quando existir relacao com `Tenant`, preserve o padrao do projeto para navigation property e foreign key.
Nao use `string tenantId` no dominio.

3. Ajuste a configuracao EF Core e o modelo relacional.

Em `Repository/Configurations`, marque `TenantId` como obrigatorio para entidades tenant-scoped.
Adicione indice para `TenantId` quando isso seguir o padrao do modulo.
Se a entidade ja possuir indice unico global, revise se ele deve se tornar composto com `TenantId`.
Configure o relacionamento com `Tenant` de forma coerente com o restante do modelo.

4. Preserve o isolamento por tenant no `CRMContext` e nos repositories.

Se o projeto usar query filter para a entidade, aplique ou ajuste o filtro com `ITenantProvider`.
Em repositories EF Core, mantenha consultas, updates e deletes restritos ao tenant atual.
Em Dapper e stored procedures, inclua `tenantId` nos parametros e filtros das entidades tenant-scoped.
Nao desabilite filtro de tenant sem motivo global explicito.

5. Ajuste Application, DTOs e mapeamentos.

Se a entidade for tenant-scoped, resolva `tenantId` pelo contexto autenticado, normalmente via `ITenantProvider`.
Nao trate `tenantId` vindo do request como fonte de verdade quando o backend puder resolve-lo.
Atualize mapeamentos e validacoes para garantir que a entidade receba `tenantId` antes de persistir.
Se a entidade for global, mantenha os DTOs e o fluxo sem `tenantId` artificial.

6. Planeje a migration com dados existentes.

Quando a tabela ja existir, nao adicione apenas a coluna e marque como obrigatoria sem pensar na carga atual.
Defina como os registros antigos receberao `tenantId`, se havera backfill, valor temporario controlado ou etapa intermediaria de migracao.
Revise impacto em indices, foreign keys, unique constraints e procedures dependentes.

7. Valide a refatoracao como mudanca estrutural.

Prefira build estreito do projeto afetado quando a mudanca estiver localizada.
Se a refatoracao atravessar varias camadas ou tocar migration, rode `dotnet build backend/crm.sln`.

## Regras Operacionais Deste Projeto

- Entidades principais devem ser tenant-scoped por padrao; entidades globais sao excecao explicita
- `TenantId` deve ser `Guid` no dominio, na persistencia e nas assinaturas internas
- `tenantId` de entidades tenant-scoped deve vir do contexto autenticado ou de `ITenantProvider`
- Nao aceite `tenantId` arbitrario do cliente quando o backend puder resolve-lo
- Repositorios devem manter isolamento por tenant em leitura e escrita
- Query filters, indices e unique constraints devem ser revisados quando `tenantId` entrar na entidade
- Stored procedures e comandos Dapper de entidades tenant-scoped devem receber `tenantId` quando necessario
- A migration deve considerar dados existentes e nao apenas o schema ideal
- Preserve mensagens em portugues quando o modulo ja usa portugues

## Resultado Esperado

Ao final, a entidade refatorada deve ficar consistente nestes pontos:

- `TenantId` presente em todas as entidades tenant-scoped relevantes
- configuracao EF Core coerente com relacionamento, indice e obrigatoriedade
- isolamento por tenant preservado em `CRMContext`, repositories e procedures
- Application e DTOs sem dependencia de `tenantId` arbitrario do cliente
- migration segura para dados existentes
- entidades globais mantidas fora do fluxo de tenant quando isso for intencional

## Validacao

1. Confira se cada entidade alterada foi classificada corretamente como tenant-scoped ou global.
2. Confira se `TenantId` foi adicionado como `Guid` e marcado como obrigatorio quando a entidade nao for global.
3. Confira se indices e unicidade foram revisados para o novo contexto multi-tenant.
4. Confira se repositories, query filters, procedures e comandos Dapper respeitam `tenantId`.
5. Confira se a Application resolve `tenantId` internamente e nao a partir de entrada arbitraria do cliente.
6. Execute `dotnet build backend/crm.sln` ou um build mais estreito do projeto afetado.

## Referencias

- [Checklist de classificacao e impacto](./references/tenant-classification-checklist.md)
- [Notas de migration e backfill](./references/migration-backfill-notes.md)
- [Template de refatoracao tenant-scoped](./assets/tenant-refactor-template.md)
