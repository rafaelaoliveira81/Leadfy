# Checklist Por Camada

Use este checklist durante a implementacao para evitar desvio de responsabilidade entre camadas.

## Api

- rota e verbo HTTP coerentes
- `ActionResult` ou `ActionResult<T>` apropriado
- `Authorize` quando necessario
- XML comments atualizados
- `ProducesResponseType` alinhado ao retorno real
- controller sem regra de negocio

## Application

- validacao de entrada feita cedo
- mapeamento entre DTO e entidade explicito
- orquestracao entre repositorios e services concentrada aqui
- reutilizacao de DTOs e interfaces existentes quando possivel
- excecoes coerentes com o padrao da feature

## Service

- usado apenas quando ha regra de dominio clara ou reutilizavel
- sem responsabilidade de contrato HTTP
- sem acoplamento desnecessario ao controller

## Repository

- apenas acesso a dados
- sem regra de negocio
- mesmo padrao do modulo: EF Core ou Dapper/stored procedure
- nova conexao por chamada quando usar Dapper
- parametros alinhados a assinatura real da stored procedure

## DI

- novos contratos registrados em `backend/Api/Program.cs`
- sem alterar CORS, JWT ou Swagger sem necessidade

## Validacao Final

- arquivos alterados condizem com o escopo real da feature
- nao houve atalho entre camadas
- build executado apos a edicao
