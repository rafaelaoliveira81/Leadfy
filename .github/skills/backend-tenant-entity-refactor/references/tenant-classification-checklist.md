# Checklist De Classificacao E Impacto

Use este checklist antes de espalhar `tenantId` por uma entidade existente.

## Classificacao Da Entidade

- A entidade representa dado de negocio pertencente a um tenant especifico?
- Existe algum caso real em que o mesmo registro deva ser visivel para todos os tenants?
- Ha outras entidades vizinhas no modulo que ja sao tenant-scoped e indicam o mesmo tratamento?
- A entidade participa de relacoes com tabelas que ja possuem `TenantId`?

Se as respostas apontarem para isolamento por cliente, trate a entidade como tenant-scoped.
Se a entidade for excecao global, documente isso no raciocinio da mudanca para evitar propagacao indevida de `tenantId`.

## Impacto Minimo A Revisar

- Entidade em `Domain/Entities`
- Configuracao EF Core em `Repository/Configurations`
- `CRMContext` e filtros globais da entidade
- Repositories EF Core e Dapper
- Stored procedures e comandos SQL dependentes
- DTOs e mapeamentos na `Application`
- Fluxo de resolucao de `tenantId` via contexto autenticado
- Migration e tratamento de dados legados

## Perguntas De Seguranca Estrutural

- A unicidade atual precisa virar composta com `TenantId`?
- Existe indice global que passa a produzir colisao entre tenants?
- Leituras, updates e deletes continuam restritos ao tenant atual?
- Algum DTO ainda aceita `tenantId` vindo do request sem necessidade?
- O tenant autenticado continua sendo a fonte de verdade da gravacao?

## Sinal De Que A Refatoracao Esta Incompleta

- `TenantId` foi adicionado na entidade, mas nao nas consultas
- Migration adiciona coluna obrigatoria sem backfill para dados existentes
- Repository continua buscando por identificador global sem filtrar por tenant
- Application ainda copia `tenantId` do request para a entidade
- Indices unicos permanecem globais em dados que agora sao tenant-scoped
