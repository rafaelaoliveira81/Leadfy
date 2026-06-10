---
name: frontend-service-api-integration
description: "Orienta integracoes de API no frontend React do Leadfy. Use para criar ou ajustar services em src/services, payloads, cliente Axios, tratamento com mapApiError, normalizacao em utils e preservacao de campos que o backend exige em updates."
argument-hint: "Descreva o service, endpoint, payload ou integracao de API a ajustar"
user-invocable: true
---

# Frontend Service API Integration

Use esta skill quando a tarefa estiver centrada na integracao do frontend com a API.
Ela cobre services, payloads, cliente HTTP, tratamento de erros e preservacao de campos esperados pelo backend.

## Papel Desta Skill No Conjunto

Esta skill cobre o slice de `services` e payload.
Use-a quando o ponto dominante estiver em HTTP, contrato de payload, update parcial ou integracao com endpoint.

## Quando Usar

- Criar um novo service em `src/services`
- Ajustar endpoint, verbo ou payload em um service existente
- Corrigir update parcial que precisa preservar campos nao editados
- Ajustar tratamento de erro com `mapApiError`
- Mover normalizacao de payload para `utils` quando isso ja seguir o padrao local

## Nao Use Quando

- A mudanca principal estiver na UI da page
- O foco principal for autenticacao ou guardas de rota
- O centro da mudanca for o Kanban como fluxo de tela completo

## Antes De Editar

1. Leia o service equivalente mais proximo.
2. Confirme o contrato real do endpoint backend.
3. Verifique se a UI precisa preservar campos nao editados no update.
4. Confirme se ja existe utilitario de normalizacao no modulo.

Consulte:

- [Checklist de service](./references/service-checklist.md)
- [Notas de preservacao de payload](./references/payload-preservation-notes.md)
- [Template de payload](./assets/payload-template.md)

## Procedimento

1. Mantenha HTTP dentro de `services`.
2. Monte payloads coerentes com o contrato esperado pelo backend.
3. Preserve campos obrigatorios quando a UI editar apenas parte da entidade.
4. Use `mapApiError` no mesmo estilo do service vizinho.
5. Deixe formatacao e normalizacao em `utils` quando esse padrao ja existir.
6. Valide com `npm run build` em `crm-app`.

## Regras Operacionais Deste Projeto

- Services devem conter apenas comunicacao HTTP e tratamento tecnico de erro
- Atualizacoes parciais nao devem perder campos que o backend espera
- `mapApiError` deve ser preservado quando ja usado no modulo
- `HTTPClient` central continua sendo o ponto de entrada para requests Axios

## Referencias

- [Checklist de service](./references/service-checklist.md)
- [Notas de preservacao de payload](./references/payload-preservation-notes.md)
- [Template de payload](./assets/payload-template.md)
