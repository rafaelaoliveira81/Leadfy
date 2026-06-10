# EF Core Vs Dapper Neste Projeto

Use este guia para decidir como implementar o acesso a dados no backend do Leadfy.

## Regra Principal

Nao escolha EF Core ou Dapper por preferencia pessoal.
Primeiro, inspecione como a feature equivalente ja acessa dados.

## Quando Manter EF Core

Prefira EF Core quando o modulo ja usa:

- consultas sobre `DbSet`
- `Include` para navegacao entre entidades
- filtros, ordenacao e materializacao em LINQ
- leitura de colecoes e agregacao simples no contexto da entidade

Sinais tipicos no projeto:

- uso de `_context.<Entidade>`
- `Include`, `Where`, `OrderBy`, `ToListAsync`

## Quando Manter Dapper

Prefira Dapper quando o modulo ja usa:

- stored procedures
- funcoes SQL
- chamadas com `ExecuteAsync`, `ExecuteScalarAsync`, `QueryAsync` ou `QueryFirstOrDefaultAsync`
- fluxo de escrita ou leitura que ja depende de contrato SQL definido no banco

Sinais tipicos no projeto:

- nome de procedure como `sp_CreateOpportunity`
- `commandType: System.Data.CommandType.StoredProcedure`
- objeto anonimo de parametros

## Regra De Conexao Para Dapper

Em metodos Dapper, crie uma nova `SqlConnection` por chamada usando a infraestrutura do projeto.
Nao use ou descarte diretamente `Database.GetDbConnection()` do `DbContext`, porque isso ja causou falhas intermitentes.

## Regra De Parametros Para Procedures

- confirme a assinatura real da procedure antes de editar
- nao inclua parametro extra apenas porque ele existe na entidade
- nao omita parametro exigido pela procedure
- preserve nomes compativeis com o contrato SQL ja usado pelo modulo

Exemplo pratico conhecido no projeto:

- `OpportunityRepo.UpdateAsync` deve seguir a assinatura atual de `sp_UpdateOpportunity`
- adicionar `UserId` extra nesse metodo causa erro de muitos argumentos

## Limite Da Camada Repository

Mesmo quando a consulta vier do banco em formato bruto, o repository deve parar no acesso e mapeamento basico.
Se a feature ja faz desserializacao ou composicao rica na `Application`, preserve esse desenho.
