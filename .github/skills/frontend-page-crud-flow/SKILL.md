---
name: frontend-page-crud-flow
description: "Cria ou ajusta telas CRUD no frontend React do Leadfy. Use para pages com listagem, filtro, paginacao, modal de create/edit/delete, validacao local, toast, Sidebar, ListingHeader e CSS Modules no padrao atual do projeto."
argument-hint: "Descreva a tela CRUD, listagem ou modal a implementar ou ajustar"
user-invocable: true
---

# Frontend Page CRUD Flow

Use esta skill quando a mudanca estiver centrada em uma tela CRUD no padrao repetido do projeto.
Ela cobre listagem, filtro, paginacao, modal, formulario, submit, toast e estados locais tipicos das paginas como Leads, Products, Users e Prompts.

## Papel Desta Skill No Conjunto

Esta skill cobre o slice de tela CRUD.
Use-a quando o ponto dominante estiver na page e em seu fluxo de listagem e formulario.
Se a mudanca cruzar varias partes do frontend sem centro claro, use `frontend-feature-flow`.

## Quando Usar

- Criar uma nova tela CRUD seguindo o padrao do projeto
- Ajustar listagem, filtro de status ou paginacao
- Ajustar modais de create, edit, activate, deactivate ou delete
- Padronizar validacao local e toasts de uma page CRUD

## Nao Use Quando

- A mudanca principal estiver em `services`
- O foco principal for autenticacao ou rotas
- O fluxo principal for o Kanban

## Antes De Editar

1. Leia uma page CRUD equivalente, como Leads ou Products.
2. Confirme service, estados locais, modais e CSS Module associados.
3. Reuse `Sidebar`, `ListingHeader`, `Button` e padroes de toast existentes.

Consulte:

- [Checklist CRUD](./references/crud-checklist.md)
- [Template de page](./assets/page-template.md)

## Procedimento

1. Monte o estado local da tela.
2. Organize fetch, submit e confirmacoes de modal.
3. Valide formulario localmente antes do submit.
4. Use o service correspondente para create, update, activate, deactivate ou delete.
5. Recarregue a listagem apos sucesso e preserve toasts coerentes.
6. Valide com `npm run build` em `crm-app`.

## Regras Operacionais Deste Projeto

- Pages seguem `pages/Feature/Feature.js`
- CSS Modules seguem o arquivo underscore-prefixed da page
- Estados comuns como `isLoading`, `isSaving`, item selecionado e modal aberta devem ficar previsiveis
- Validacao simples fica na propria page quando esse ja for o padrao da tela
- Chamadas de API continuam em `services`

## Referencias

- [Checklist CRUD](./references/crud-checklist.md)
- [Template de page](./assets/page-template.md)
