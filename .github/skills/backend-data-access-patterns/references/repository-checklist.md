# Checklist De Repository

Use este checklist antes de concluir qualquer alteracao na camada Repository.

## Responsabilidade

- o metodo faz apenas persistencia, leitura ou mapeamento basico
- nao ha regra de negocio no repository
- nao ha traducao de excecao para HTTP nessa camada

## Escolha Tecnica

- a escolha entre EF Core e Dapper segue o padrao da feature
- a implementacao nova combina com os metodos vizinhos do mesmo repository

## Dapper

- usa nova conexao por chamada
- o comando esta marcado corretamente como stored procedure quando aplicavel
- o objeto de parametros bate com a assinatura real do banco
- nao ha parametro extra nem faltando

## EF Core

- usa `DbSet` correto
- `Include` e filtros seguem o mesmo estilo do modulo
- ordenacao e materializacao estao claras
- a consulta nao carrega regra de negocio desnecessaria

## Contratos

- assinatura do metodo combina com a interface correspondente
- tipo de retorno combina com o consumo na `Application`
- DTO bruto ou entidade retornada seguem o padrao existente

## Validacao Final

- a mudanca nao exige ajuste oculto em Application ou DTO sem ter sido tratado
- o metodo compila com o restante do projeto
- build executado apos a alteracao
