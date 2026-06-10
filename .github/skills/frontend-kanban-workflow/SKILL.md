---
name: frontend-kanban-workflow
description: "Orienta alteracoes no Kanban do Leadfy. Use para board por stages, drag and drop, modal de oportunidade, interacoes, prompts, action plans, consumo de APIs relacionadas e preservacao de payload completo em updates de oportunidade."
argument-hint: "Descreva o ajuste no Kanban, drag and drop, interacao ou payload da oportunidade"
user-invocable: true
---

# Frontend Kanban Workflow

Use esta skill quando a tarefa estiver centrada na page `Kanban` e em seu fluxo stateful.
Ela cobre board por stages, drag and drop, modal de oportunidade, interacoes, prompts, action plans e updates que precisam preservar o payload esperado pelo backend.

## Papel Desta Skill No Conjunto

Esta skill cobre o slice do Kanban.
Use-a quando a mudanca dominante estiver no board, no modal de oportunidade ou nas integracoes especificas do Kanban.

## Quando Usar

- Ajustar drag and drop entre stages
- Corrigir rollback local apos falha de persistencia
- Ajustar modal de oportunidade
- Revisar fluxo de interacoes, optimize interaction ou action plans
- Corrigir payload de update de oportunidade no Kanban

## Nao Use Quando

- A mudanca principal estiver em uma tela CRUD comum
- O foco principal for apenas `AuthContext` ou guardas de rota
- A tarefa estiver concentrada em um service generico fora do fluxo do Kanban

## Antes De Editar

1. Leia a page `pages/Kanban/Kanban.js`.
2. Confirme quais services do Kanban participam do fluxo.
3. Verifique se a mudanca afeta board, modal, interactions, prompts ou action plans.
4. Confirme o payload real esperado pelo backend antes de mudar updates.

Consulte:

- [Checklist do Kanban](./references/kanban-checklist.md)
- [Notas de preservacao de payload](./references/payload-preservation-notes.md)

## Procedimento

1. Preserve a carga do board por stages.
2. Em drag and drop, mantenha atualizacao otimista com rollback em caso de erro.
3. No modal, preserve coerencia entre oportunidade selecionada, interacoes e action plans.
4. Em updates de oportunidade, envie payload completo quando o backend nao aceitar parcial.
5. Em optimize interaction, alinhe payload e leitura da resposta ao contrato real da API.
6. Valide com `npm run build` em `crm-app`.

## Regras Operacionais Deste Projeto

- o board e carregado por stage via services do modulo
- falha de persistencia no drag and drop deve restaurar o estado local
- update de oportunidade no Kanban deve preservar campos que o backend espera
- optimize interaction usa payload `{ content }` e o retorno deve ser lido de acordo com a resposta real da API

## Referencias

- [Checklist do Kanban](./references/kanban-checklist.md)
- [Notas de preservacao de payload](./references/payload-preservation-notes.md)
