# Guia Para Construir Skills Do Frontend

Este arquivo documenta como criar skills reutilizaveis para o frontend do Leadfy.
O objetivo e transformar fluxos recorrentes do `crm-app/src` em skills acionaveis, sem misturar escopos diferentes dentro da mesma skill.

## Objetivo Das Skills

No frontend, as instructions ja cobrem regras amplas de stack, estrutura, servicos, rotas, contexto, componentes e CSS Modules.
As skills devem ser usadas para fluxos especificos, repetitivos e orientados a entrega.

Use skill quando o trabalho exigir:

- varios passos encadeados em `pages`, `services`, `context`, `routes` ou `utils`
- decisao guiada pelo padrao real do modulo
- uso de arquivos auxiliares, templates ou checklists
- repeticao frequente entre telas, servicos e fluxos autenticados

Nao use skill quando o conteudo for apenas uma regra geral do frontend.
Nesse caso, a regra deve continuar em `.github/copilot-instructions.md` ou `.github/instructions/frontend.instructions.md`.

## Estrutura Recomendada

Cada skill deve ficar em uma pasta propria dentro de `.github/skills`.

```text
.github/skills/
  frontend-feature-flow/
    SKILL.md
    references/
      feature-scope.md
      layer-checklist.md
    assets/
      feature-template.md
  frontend-page-crud-flow/
    SKILL.md
    references/
      crud-checklist.md
```

## Regras De Formato

Cada skill precisa de um arquivo `SKILL.md` com frontmatter YAML valido.

Exemplo minimo:

```md
---
name: frontend-feature-flow
description: "Cria ou altera uma feature completa no frontend React do Leadfy. Use quando precisar ajustar page, component, service, route, context, utils e CSS Modules dentro do crm-app/src."
argument-hint: "Descreva a feature frontend ou fluxo a implementar"
user-invocable: true
---

# Frontend Feature Flow

## Quando Usar

- Criar uma nova feature frontend
- Expandir uma feature existente em varias partes do crm-app/src

## Procedimento

1. Identificar a tela ou fluxo equivalente.
2. Confirmar page, component, service e route envolvidos.
3. Implementar a mudanca sem quebrar o padrao existente.
4. Validar com build.
```

## Regras Importantes Para Este Projeto

As skills do frontend devem refletir o desenho real do Leadfy:

- React com React Router, Context API, Axios, React Bootstrap, React Toastify e CSS Modules
- paginas em `pages/Feature/Feature.js`
- componentes reutilizaveis em `components/Component/Component.js`
- servicos HTTP em `services/*Api.js`
- autenticacao centralizada em `AuthContext` e guardas de rota
- telas CRUD com modal, toast, listagem, filtro e paginacao
- Kanban como fluxo complexo e mais stateful do frontend

Ao escrever as skills, inclua essas decisoes como criterio operacional, nao apenas como contexto.

## Pacote Inicial De Skills Recomendadas

Estas sao as skills mais uteis para comecar no frontend.

## Indice Das Skills

Use este indice para navegar direto para cada skill e escolher a mais adequada pelo ponto dominante da tarefa.

| Skill                              | Link Direto                                                                                               | Quando Usar                                                                                                      |
| ---------------------------------- | --------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------- |
| `frontend-feature-flow`            | [`.github/skills/frontend-feature-flow/SKILL.md`](./frontend-feature-flow/SKILL.md)                       | Quando a entrega atravessa varias partes do `crm-app/src` e nao ha um unico centro dominante.                    |
| `frontend-page-crud-flow`          | [`.github/skills/frontend-page-crud-flow/SKILL.md`](./frontend-page-crud-flow/SKILL.md)                   | Quando o centro da mudanca esta em uma tela CRUD com listagem, modal, form, toast e paginacao.                   |
| `frontend-service-api-integration` | [`.github/skills/frontend-service-api-integration/SKILL.md`](./frontend-service-api-integration/SKILL.md) | Quando o centro da mudanca esta em `services`, payloads, cliente HTTP e integracao com a API.                    |
| `frontend-auth-routing`            | [`.github/skills/frontend-auth-routing/SKILL.md`](./frontend-auth-routing/SKILL.md)                       | Quando o centro da mudanca esta em `AuthContext`, token, rotas protegidas ou comportamento autenticado.          |
| `frontend-kanban-workflow`         | [`.github/skills/frontend-kanban-workflow/SKILL.md`](./frontend-kanban-workflow/SKILL.md)                 | Quando o centro da mudanca esta no Kanban, drag and drop, modal de oportunidade, interacoes e payloads parciais. |

## Regra Rapida De Escolha

Se a tarefa for ampla e nao houver um centro dominante claro, comece por `frontend-feature-flow`.
Se houver um centro dominante claro, prefira a skill especializada correspondente e use a skill ampla apenas quando a mudanca realmente cruzar varios pontos do frontend.

### 1. `frontend-feature-flow`

Skill principal para criar ou alterar uma feature completa no frontend.

Use quando a tarefa envolver uma ou mais destas etapas:

- criar ou alterar pagina
- ajustar componente reutilizavel
- atualizar service e consumo de API
- alterar route ou fluxo autenticado como parte da entrega
- mexer em utilitario, normalizacao de payload e CSS Module no mesmo fluxo

Essa skill deve orientar o agente a:

1. localizar uma tela ou fluxo equivalente ja existente
2. identificar o ponto que realmente controla o comportamento no frontend
3. seguir o padrao de pastas, nomes e imports do modulo mais proximo
4. evitar logica de API dentro da pagina quando isso pertence ao service
5. evitar criar abstrações novas sem padrao equivalente no projeto
6. validar com `npm run build` em `crm-app`

Arquivos auxiliares sugeridos:

- `references/feature-scope.md`
- `references/layer-checklist.md`
- `assets/feature-template.md`

### 2. `frontend-page-crud-flow`

Skill especializada em telas CRUD no padrao atual do projeto.

Use quando a tarefa envolver:

- listagem com filtro e paginacao
- modal de criacao, edicao, ativacao ou exclusao
- formulario com validacao local
- toast de sucesso ou erro

Essa skill deve orientar o agente a verificar:

1. padrao de page em `pages/Feature/Feature.js`
2. uso consistente de `Sidebar`, `ListingHeader`, `Modal` e `toast`
3. estado local para loading, saving, item selecionado e modal aberto
4. validacao local simples antes do submit
5. estilo com CSS Module underscore-prefixed

Arquivos auxiliares sugeridos:

- `references/crud-checklist.md`
- `assets/page-template.md`

### 3. `frontend-service-api-integration`

Skill para orientar integracoes com a API no frontend.

Use quando a tarefa envolver:

- criar ou ajustar service em `src/services`
- montar payload esperado pelo backend
- preservar campos que a API exige em updates parciais
- tratar erros com `mapApiError`

Essa skill deve orientar o agente a:

1. manter HTTP e erro dentro de `services`
2. preservar payload completo quando o backend exigir campos nao editados na UI
3. separar normalizacao em `utils` quando ja houver esse padrao
4. manter o estilo do modulo ja existente no service

Arquivos auxiliares sugeridos:

- `references/service-checklist.md`
- `references/payload-preservation-notes.md`
- `assets/payload-template.md`

### 4. `frontend-auth-routing`

Skill para tarefas relacionadas a autenticacao, contexto e rotas.

Use quando a tarefa envolver:

- `AuthContext`
- token e claims no frontend
- `ProtectedRoute` e `PublicRoute`
- fluxos sensiveis a autenticacao

Essa skill deve orientar o agente a:

1. reaproveitar `AuthContext`
2. preservar `isLoading` para evitar redirecionamento prematuro
3. manter token e claims coerentes com o backend
4. concentrar guarda de rota em `routes/Rotas.js`

Arquivos auxiliares sugeridos:

- `references/auth-checklist.md`
- `references/route-guard-notes.md`

### 5. `frontend-kanban-workflow`

Skill para o fluxo complexo do Kanban.

Use quando a tarefa envolver:

- board por stages
- drag and drop
- modal de oportunidade
- interacoes, prompts e action plans
- updates parciais que exigem preservar campos esperados pelo backend

Essa skill deve orientar o agente a:

1. preservar a carga do board por stage
2. tratar rollback local em falha de drag and drop
3. manter payload completo nos updates de oportunidade
4. alinhar consumo de prompts, interacoes otimizadas e action plans com a API real

Arquivos auxiliares sugeridos:

- `references/kanban-checklist.md`
- `references/payload-preservation-notes.md`

## Como Escrever A Descricao Da Skill

A descricao e o principal gatilho de descoberta.
Ela precisa conter:

- o que a skill faz
- quando usar
- termos reais do projeto
- palavras-chave que o agente provavelmente buscara

Boas palavras-chave para este frontend:

- React
- page
- component
- route
- AuthContext
- ProtectedRoute
- service
- axios
- payload
- CSS Module
- modal
- toast
- Kanban

Evite descricoes vagas como:

- `Ajuda no frontend`
- `Skill para React`
- `Faz varias tarefas`

## O Que Colocar Em `references/`

Use `references/` para conhecimento de apoio que nao precisa ser carregado sempre.

Exemplos adequados:

- checklist de tela CRUD
- notas sobre guardas de rota
- regras de preservacao de payload
- mapa de arquivos que costumam ser alterados juntos
- comandos de validacao

## O Que Colocar Em `assets/`

Use `assets/` para material reutilizavel.

Exemplos adequados:

- template de pagina
- template de payload
- template de checklist manual
- snippets de estrutura de estado local

## Checklist De Qualidade Antes De Criar A Skill

Antes de considerar a skill pronta, confirme:

1. o nome da pasta e o campo `name` sao identicos
2. a descricao deixa claro quando a skill deve ser usada
3. o `SKILL.md` tem procedimento objetivo e acionavel
4. a skill nao duplica instrucoes globais sem adicionar workflow
5. os arquivos em `references/` e `assets/` sao referenciados com caminho relativo `./`
6. a skill menciona validacao apropriada para o frontend

## Ordem Recomendada De Implementacao

Para comecar sem dispersao, implemente nesta ordem:

1. `frontend-feature-flow`
2. `frontend-page-crud-flow`
3. `frontend-service-api-integration`
4. `frontend-auth-routing`
5. `frontend-kanban-workflow`

## Prompts Reais Para Testar Descoberta E Uso

### `frontend-feature-flow`

```text
Quero criar uma nova tela de clientes no frontend com listagem, modal de cadastro, service da API, rota protegida e CSS Module seguindo o padrao das paginas existentes.
```

```text
Preciso expandir a tela de produtos para incluir um novo campo que exige ajuste na pagina, normalizacao em utils e envio correto no service.
```

### `frontend-page-crud-flow`

```text
Crie uma tela CRUD de categorias seguindo o mesmo padrao de Leads e Products, com listagem, filtro de status, paginacao, modais de criacao e edicao e toasts de sucesso e erro.
```

```text
Revise a tela de usuarios para padronizar os estados de loading, saving, modal aberta e item selecionado sem mudar o fluxo geral da pagina.
```

### `frontend-service-api-integration`

```text
Preciso ajustar o service de oportunidades para enviar um payload completo no update, preservando campos que o backend espera mesmo quando a UI altera so parte da oportunidade.
```

```text
Crie um novo service para consumir um endpoint de dashboard, mantendo HTTP e tratamento de erro em services e deixando formatacao fora do service.
```

### `frontend-auth-routing`

```text
Revise o AuthContext e as rotas protegidas para evitar redirecionar para /login durante a restauracao do token apos refresh da pagina.
```

```text
Crie uma rota publica nova para recuperacao de senha e mantenha o comportamento atual de PublicRoute e ProtectedRoute consistente com o restante do app.
```

### `frontend-kanban-workflow`

```text
Preciso ajustar o Kanban para preservar os campos atuais da oportunidade quando o usuario editar apenas valor e data de fechamento, sem quebrar o update esperado pelo backend.
```

```text
Revise o fluxo de otimizar interacao no Kanban para enviar o payload correto, consumir a resposta real da API e atualizar o texto da interacao na UI.
```

## Prompts Limitrofes Para Testar Escolha Da Skill

Use estes prompts quando quiser testar se o agente escolhe corretamente entre `frontend-feature-flow`, `frontend-service-api-integration` e `frontend-kanban-workflow` em cenarios com ambiguidade.

### Entre `frontend-feature-flow` e `frontend-service-api-integration`

```text
Preciso adicionar um novo campo na tela de produtos e garantir que ele seja enviado corretamente para a API sem quebrar o payload atual. Ajuste somente o necessario no frontend e preserve o padrao do modulo.
```

```text
Analise se o problema desta tarefa esta mais no service ou na tela: o update de leads esta falhando porque o payload enviado nao bate com o backend, mas a tela tambem pode precisar normalizar melhor os dados antes do submit.
```

### Entre `frontend-feature-flow` e `frontend-kanban-workflow`

```text
Preciso ajustar o modal do Kanban para exibir um novo campo de oportunidade, enviar esse valor no update e manter o restante do fluxo intacto. Reuse o que ja existe no Kanban e altere apenas o necessario.
```

```text
Tenho uma mudanca que parece simples de UI, mas acontece dentro do Kanban e afeta o update da oportunidade e o estado local do modal. Escolha o fluxo certo para implementar isso sem quebrar o board.
```

### Entre `frontend-service-api-integration` e `frontend-kanban-workflow`

```text
Revise o update de oportunidade usado pelo Kanban: quero saber se a correcao deve ficar no service, no payload montado pela tela ou nos dois, porque alguns campos obrigatorios estao sendo perdidos.
```

```text
Preciso corrigir o fluxo de otimizar interacao no Kanban. Verifique se o problema esta no contrato do service, no payload enviado pela tela ou na leitura da resposta, e escolha a skill mais adequada para conduzir a mudanca.
```
