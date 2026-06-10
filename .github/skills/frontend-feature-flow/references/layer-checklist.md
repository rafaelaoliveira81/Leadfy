# Checklist Por Camada Do Frontend

## Pages

- tela continua focada no comportamento do screen
- estados de loading, saving e erro estao claros
- modais, filtros e paginacao seguem o padrao vizinho

## Components

- componente realmente tem papel reutilizavel ou local bem definido
- props estao coerentes e simples
- nao houve extracao desnecessaria

## Services

- chamadas HTTP ficaram em `services`
- nao ha logica de UI no service
- contrato com backend ficou coerente com payload e resposta
- `mapApiError` foi preservado quando esse ja era o padrao do modulo

## Utils

- normalizacao e formatacao ficaram fora da UI quando aplicavel
- utilitario existente foi reaproveitado antes de criar novo

## Routes E Context

- autenticacao e redirecionamento estao coerentes
- `AuthContext` so foi tocado quando realmente necessario

## Estilo

- CSS Module segue o padrao underscore-prefixed do modulo
- visual continua coerente com a tela vizinha

## Validacao Final

- a mudanca segue o padrao do modulo vizinho
- `npm run build` foi executado apos a edicao
