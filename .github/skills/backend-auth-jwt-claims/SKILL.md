---
name: backend-auth-jwt-claims
description: "Orienta tarefas de autenticacao no backend ASP.NET Core do Leadfy. Use para endpoints autenticados, uso de Authorize, leitura de claims do usuario logado, geracao e validacao de JWT, integracao com TokenService e preservacao da configuracao JWT em Program.cs."
argument-hint: "Descreva o endpoint autenticado, claim, token ou ajuste de JWT a implementar"
user-invocable: true
---

# Backend Auth JWT Claims

Use esta skill quando a tarefa envolver autenticacao, autorizacao ou contexto do usuario logado no backend.
Ela serve para manter coerencia entre `Authorize`, leitura de claims, geracao de token e configuracao JWT sem espalhar parse manual de claims pelos controllers.

## Papel Desta Skill No Conjunto

Esta skill cobre o slice de autenticacao e contexto do usuario logado.
Use-a quando o ponto dominante da tarefa estiver em `Authorize`, claims, emissao de JWT, configuracao de autenticacao ou passagem do `userId` autenticado para a regra de negocio.
Se a mudanca principal for o contrato HTTP sem impacto real de autenticacao, prefira `backend-endpoint-swagger`.

## Quando Usar

- Criar ou ajustar endpoint autenticado
- Ler o usuario autenticado a partir das claims
- Gerar JWT na autenticacao
- Validar impacto de mudanca em `Program.cs` relacionada a JWT
- Corrigir fluxo que depende do usuario logado na regra de negocio

## Nao Use Quando

- A tarefa principal for Swagger ou contrato HTTP sem impacto de autenticacao
- A mudanca principal estiver em Repository ou acesso a dados
- O foco principal for validacao de Application sem dependencia do usuario autenticado
- A entrega principal for uma feature multi-camada sem autenticacao como centro dominante

Nesses casos, prefira a skill especializada no ponto principal da mudanca.

## Antes De Editar

1. Leia o controller ou fluxo autenticado equivalente mais proximo.
2. Confirme se a acao precisa apenas de `Authorize` ou tambem do usuario logado na regra de negocio.
3. Verifique se a claim necessaria ja existe no token atual.
4. Inspecione `Program.cs` e `TokenService` antes de alterar configuracao JWT.

Consulte:

- [Checklist de autenticacao](./references/auth-checklist.md)
- [Notas de configuracao JWT](./references/jwt-config-notes.md)

## Procedimento

1. Defina o comportamento de autenticacao do endpoint.

Confirme se a acao deve ser publica ou protegida por `Authorize`.
Se o endpoint depende do usuario logado, trate isso como parte explicita do contrato e do fluxo de negocio.

2. Reutilize o helper de claims existente.

Quando precisar do identificador do usuario autenticado, use o helper ja existente em vez de parse manual de `ClaimsPrincipal` dentro do controller.
Evite repetir o nome da claim em cada acao.

3. Preserve a coerencia entre token e consumo das claims.

Se uma claim e usada no backend, confirme que ela esta sendo emitida pelo `TokenService` com o nome correto.
Nao altere nome de claim ou estrutura do token sem revisar o consumo existente.

4. Mantenha a configuracao JWT centralizada.

Se a tarefa tocar em `backend/Api/Program.cs`, preserve o modelo atual de `AddAuthentication`, `AddJwtBearer` e `TokenValidationParameters`.
Evite alterar issuer, audience, secret ou swagger security sem necessidade concreta.

5. Nao empurre autenticacao para a camada errada.

O controller deve aplicar `Authorize` e obter o usuario autenticado quando necessario.
A Application pode receber o `userId` ja resolvido para usar na regra de negocio.
Nao espalhe parse de claim por repositories ou camadas que nao deveriam conhecer o request HTTP.

6. Valide a mudanca.

Se houver verificacao mais estreita do fluxo autenticado, use-a primeiro.
Caso contrario, rode `dotnet build backend/crm.sln` ou um build mais estreito do projeto afetado.

## Regras Operacionais Deste Projeto

- Reutilize o helper `User.GetAuthenticatedUserId()` quando o fluxo exigir `usuarioId`
- Preserve a claim `usuarioId` emitida pelo `TokenService` quando ela continuar sendo consumida pelos controllers
- Use `Authorize` de forma coerente com o contrato real do endpoint
- Nao faca parse manual de claims em cada controller
- Preserve a configuracao JWT em `backend/Api/Program.cs` no menor escopo possivel
- Se a mudanca impactar Swagger por causa de autenticacao, mantenha a documentacao de Bearer coerente com a configuracao existente

## Resultado Esperado

Ao final, o fluxo autenticado deve ficar consistente nestes pontos:

- endpoint protegido quando necessario
- claims consumidas por helper centralizado
- token emitido com dados coerentes para o backend
- configuracao JWT preservada ou alterada de forma minima e consciente
- regra de negocio recebendo apenas o contexto necessario, como `userId`

## Validacao

1. Confira se o endpoint usa `Authorize` quando realmente precisa.
2. Confira se a claim usada pelo fluxo existe no token emitido.
3. Execute `dotnet build backend/crm.sln` se nao houver validacao mais estreita.

## Referencias

- [Checklist de autenticacao](./references/auth-checklist.md)
- [Notas de configuracao JWT](./references/jwt-config-notes.md)
