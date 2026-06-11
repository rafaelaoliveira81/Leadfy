# Notas De Migration E Backfill

Use estas notas quando a tabela alvo ja existir em producao ou ambiente compartilhado.

## Perguntas Antes Da Migration

- A tabela ja possui registros sem `TenantId`?
- Existe origem confiavel para descobrir o tenant de cada linha antiga?
- O backfill pode ser feito na migration ou precisa de etapa operacional separada?
- Ha indices, foreign keys ou procedures que dependem do schema atual?

## Estrategias Seguras

### 1. Coluna temporariamente nullable com backfill controlado

Use quando os dados antigos precisam ser classificados antes de tornar a coluna obrigatoria.

Passos comuns:

1. Adicionar a coluna `TenantId` como nullable.
2. Preencher os registros existentes com base em relacionamento confiavel.
3. Revisar duplicidades que surgem ao introduzir unicidade por tenant.
4. Tornar a coluna obrigatoria em etapa seguinte.

### 2. Backfill direto na migration

Use apenas quando houver regra deterministica e segura para inferir o tenant antigo.

Cuidados:

- registrar claramente a origem do valor aplicado
- revisar impacto em performance quando a tabela for grande
- evitar valores placeholder sem semantica de negocio

### 3. Etapa operacional separada

Use quando a atribuicao do tenant depender de validacao manual, lote externo ou dados de outro sistema.

Nesse caso, a skill deve deixar explicito que o schema ideal depende de um plano de implantacao em fases.

## Revisoes Obrigatorias

- indices compostos com `TenantId`
- foreign keys para entidades tenant-scoped
- queries Dapper e stored procedures
- filtros globais ou clausulas `WHERE` por tenant
- scripts de seed e dados de teste

## Evite

- adicionar `TenantId` como `NOT NULL` sem estrategia para dados antigos
- assumir que um unico tenant padrao serve para todos os registros legados
- atualizar schema sem revisar unicidade e consultas
- aceitar `tenantId` do cliente como atalho para compensar backfill incompleto
