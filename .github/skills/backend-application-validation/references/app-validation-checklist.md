# Checklist Da Application

Use este checklist antes de concluir alteracoes na camada `Application`.

## Validacao De Entrada

- request validado logo no inicio do fluxo
- campos obrigatorios tratados
- limites, tamanhos, formatos e datas validados quando aplicavel
- `ArgumentException` usado quando a entrada e invalida

## Existencia E Precondicoes

- entidades dependentes carregadas antes do uso
- `KeyNotFoundException` usado quando o recurso nao existe
- metodos auxiliares de validacao mantem o fluxo principal limpo

## Mapeamento

- request para entidade em metodo dedicado quando fizer sentido
- entidade para response em metodo dedicado quando fizer sentido
- nomes de mapeamento seguem o padrao do modulo, como `MapTo...`
- nao ha mapeamento duplicado desnecessariamente em varios trechos do fluxo

## Orquestracao

- Application coordena repositories e services
- controller nao assumiu regra de negocio
- repository nao recebeu responsabilidade de fluxo
- service so e usado quando a regra for de dominio ou reutilizavel

## Contratos

- DTOs existentes foram considerados antes de criar novos
- interfaces de App continuam coerentes com a implementacao
- tipos de retorno combinam com o consumo da camada Api

## Validacao Final

- o App continua facil de ler
- excecoes estao coerentes com o modulo
- build executado apos a alteracao
