---
name: frontend-auth-routing
description: "Orienta autenticacao e rotas no frontend React do Leadfy. Use para AuthContext, token e claims no navegador, ProtectedRoute, PublicRoute, restauracao de sessao, comportamento apos refresh e fluxos sensiveis a autenticacao no crm-app/src."
argument-hint: "Descreva a rota, comportamento autenticado ou ajuste no AuthContext"
user-invocable: true
---

# Frontend Auth Routing

Use esta skill quando a tarefa envolver autenticacao no frontend, restauracao de token, claims no navegador ou guardas de rota.

## Papel Desta Skill No Conjunto

Esta skill cobre o slice de `AuthContext` e `routes/Rotas.js`.
Use-a quando o ponto dominante estiver em login, claims, redirecionamento, protecao de rota ou comportamento apos refresh.

## Quando Usar

- Ajustar `AuthContext`
- Revisar token e claims no navegador
- Ajustar `ProtectedRoute` ou `PublicRoute`
- Corrigir fluxo de redirecionamento em refresh
- Implementar tela ou rota publica/protegida com dependencia de autenticacao

## Nao Use Quando

- A mudanca principal estiver em uma tela CRUD sem impacto de autenticacao
- O foco principal for `services` gerais sem relacao com token
- O centro da mudanca for o Kanban sem impacto em guardas de rota

## Antes De Editar

1. Leia `AuthContext.js` e `routes/Rotas.js`.
2. Confirme se o fluxo depende de token, claims ou apenas de `isAuthenticated`.
3. Verifique se o comportamento de `isLoading` precisa ser preservado para evitar redirecionamento prematuro.

Consulte:

- [Checklist de autenticacao frontend](./references/auth-checklist.md)
- [Notas de guardas de rota](./references/route-guard-notes.md)

## Procedimento

1. Preserve o `AuthContext` como fonte principal de autenticacao.
2. Mantenha a restauracao de token e claims coerente com o backend.
3. Preserve `isLoading` durante leitura do token para evitar redirecionamento precoce.
4. Concentre guardas em `ProtectedRoute` e `PublicRoute`.
5. Passe para pages apenas o contexto autenticado realmente necessario.
6. Valide com `npm run build` em `crm-app`.

## Regras Operacionais Deste Projeto

- `AuthContext` centraliza token, claims, loading e login/logout
- guardas de rota dependem de `isLoading` para evitar ida indevida a `/login` durante restauracao do token
- `Routes` e `Navigate` devem continuar coerentes com o comportamento atual do app

## Referencias

- [Checklist de autenticacao frontend](./references/auth-checklist.md)
- [Notas de guardas de rota](./references/route-guard-notes.md)
