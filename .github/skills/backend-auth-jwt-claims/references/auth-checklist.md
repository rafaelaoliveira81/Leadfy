# Checklist De Autenticacao E Claims

Use este checklist antes de concluir mudancas relacionadas a autenticacao.

## Endpoint

- a acao esta publica ou protegida de forma intencional
- `Authorize` foi aplicado quando o endpoint exige autenticacao
- o contrato HTTP continua coerente com a exigencia de autenticacao

## Claims

- o fluxo usa helper existente para obter o `usuarioId`
- nao ha parse manual repetido de claims no controller
- o nome da claim consumida bate com o nome emitido no token
- o endpoint so depende das claims realmente necessarias

## Token

- o `TokenService` continua emitindo as claims esperadas
- issuer, audience e secret nao foram alterados sem necessidade
- a mudanca nao quebrou compatibilidade com controllers que usam o token atual

## Camadas

- o controller resolve o contexto autenticado e passa apenas o necessario adiante
- a Application recebe dados como `userId` quando a regra exigir
- repository nao depende de `ClaimsPrincipal` ou contexto HTTP

## Validacao Final

- autenticacao e autorizacao continuam coerentes com o endpoint
- helper e configuracao JWT foram reaproveitados quando possivel
- build executado apos a alteracao
