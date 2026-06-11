# Template De Refatoracao Tenant-Scoped

Use este roteiro curto para planejar ou revisar a alteracao.

## Entidade Alvo

- Entidade:
- Modulo:
- Classificacao: tenant-scoped ou global
- Motivo da classificacao:

## Arquivos Afetados

- `Domain/Entities/...`
- `Repository/Configurations/...`
- `Repository/Repositories/...`
- `Application/DTO/...`
- `Application/Applications/...`
- Migration:

## Mudancas Estruturais

- Adicionar `TenantId` como `Guid`
- Ajustar relacionamento com `Tenant`, se existir
- Revisar indice unico e chave composta
- Revisar filtros de leitura e escrita por tenant

## Origem Do Tenant

- `tenantId` resolvido por `ITenantProvider` ou contexto autenticado?
- Algum request ainda envia `tenantId` sem necessidade?
- Onde ocorre o mapeamento da entrada para a entidade?

## Migration E Dados Legados

- Tabela ja existe com dados?
- Estrategia de backfill:
- Impacto em indices, procedures e constraints:
- Ordem de implantacao:

## Validacao

- Build estreito do projeto afetado ou `dotnet build backend/crm.sln`
- Conferencia de filtro por tenant em leitura, update e delete
- Revisao de DTOs para evitar `tenantId` arbitrario do cliente