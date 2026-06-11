# Guia Para Construir Skills Do Backend

## Indice Mestre

Use este arquivo como ponto de entrada para os dois guias de skills do projeto.

| Guia     | Link                                                        | Quando Consultar                                                                |
| -------- | ----------------------------------------------------------- | ------------------------------------------------------------------------------- |
| Backend  | [`.github/skills/README.md`](./README.md)                   | Quando for criar, revisar ou evoluir skills do backend ASP.NET Core.            |
| Frontend | [`.github/skills/README-frontend.md`](./README-frontend.md) | Quando for criar, revisar ou evoluir skills do frontend React em `crm-app/src`. |

## Pacotes De Skills

| Area     | Skills Principais                                                                                                                               |
| -------- | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| Backend  | `backend-feature-flow`, `backend-endpoint-swagger`, `backend-data-access-patterns`, `backend-application-validation`, `backend-auth-jwt-claims`, `backend-tenant-entity-refactor` |
| Frontend | `frontend-feature-flow`, `frontend-page-crud-flow`, `frontend-service-api-integration`, `frontend-auth-routing`, `frontend-kanban-workflow`     |

Este arquivo documenta como criar skills reutilizaveis para o backend do Leadfy.
O objetivo e transformar fluxos recorrentes do projeto em skills acionaveis, em vez de depender apenas de instrucoes globais.

## Objetivo Das Skills

Neste repositorio, as instructions ja cobrem regras amplas de arquitetura e estilo.
As skills devem ser usadas para fluxos especificos, repetitivos e orientados a entrega.

Use skill quando o trabalho exigir:

- varios passos encadeados
- decisao guiada por padrao do projeto
- uso de arquivos auxiliares, templates ou referencias
- repeticao frequente em features diferentes

Nao use skill quando o conteudo for apenas uma regra geral do projeto.
Nesse caso, a regra deve continuar em `.github/copilot-instructions.md` ou `.github/instructions/*.instructions.md`.

## Estrutura Recomendada

Cada skill deve ficar em uma pasta propria dentro de `.github/skills`.

```text
.github/skills/
  backend-feature-flow/
    SKILL.md
    references/
      controller-checklist.md
      app-checklist.md
      repository-checklist.md
    assets/
      endpoint-template.md
  backend-endpoint-swagger/
    SKILL.md
    references/
      swagger-checklist.md
```

## Regras De Formato

Cada skill precisa de um arquivo `SKILL.md` com frontmatter YAML valido.

Exemplo minimo:

```md
---
name: backend-feature-flow
description: "Cria ou altera uma feature completa no backend do Leadfy. Use quando precisar implementar endpoint, DTO, App, repositorio, interfaces e DI seguindo o fluxo Api -> Application -> Service -> Repository."
argument-hint: "Descreva a feature ou endpoint a implementar"
user-invocable: true
---

# Backend Feature Flow

## Quando Usar

- Criar uma nova feature backend
- Expandir uma feature existente em varias camadas

## Procedimento

1. Identificar a feature e a camada controladora.
2. Atualizar DTOs e interfaces necessarias.
3. Implementar App, repositorio e controller.
4. Atualizar DI em `backend/Api/Program.cs`.
5. Validar com build.
```

## Regras Importantes Para Este Projeto

As skills do backend devem refletir o desenho real do Leadfy:

- fluxo principal em camadas: `Api -> Application -> Service -> Repository`
- controllers finos e orientados a contrato HTTP
- regras de negocio e validacoes na camada `Application` ou `Service`
- persistencia concentrada em `Repository`
- uso misto de EF Core e Dapper/stored procedures conforme a feature
- registro manual de dependencias em `backend/Api/Program.cs`
- documentacao Swagger e XML comments nos controllers

Ao escrever as skills, inclua sempre essas decisoes como criterio operacional, nao apenas como contexto.

## Pacote Inicial De Skills Recomendadas

Estas sao as skills mais uteis para comecar no backend.

## Indice Das Skills

Use este indice para navegar direto para cada skill e escolher a mais adequada pelo ponto dominante da tarefa.

| Skill                            | Link Direto                                                                                           | Quando Usar                                                                                  |
| -------------------------------- | ----------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- |
| `backend-feature-flow`           | [`.github/skills/backend-feature-flow/SKILL.md`](./backend-feature-flow/SKILL.md)                     | Quando a entrega atravessa varias camadas e nao ha um unico centro dominante.                |
| `backend-endpoint-swagger`       | [`.github/skills/backend-endpoint-swagger/SKILL.md`](./backend-endpoint-swagger/SKILL.md)             | Quando o centro da mudanca esta no contrato HTTP, controller, status codes ou Swagger.       |
| `backend-data-access-patterns`   | [`.github/skills/backend-data-access-patterns/SKILL.md`](./backend-data-access-patterns/SKILL.md)     | Quando o centro da mudanca esta em Repository, EF Core, Dapper, stored procedure ou conexao. |
| `backend-application-validation` | [`.github/skills/backend-application-validation/SKILL.md`](./backend-application-validation/SKILL.md) | Quando o centro da mudanca esta em validacao, mapeamento e orquestracao na Application.      |
| `backend-auth-jwt-claims`        | [`.github/skills/backend-auth-jwt-claims/SKILL.md`](./backend-auth-jwt-claims/SKILL.md)               | Quando o centro da mudanca esta em `Authorize`, claims, JWT ou usuario autenticado.          |
| `backend-tenant-entity-refactor` | [`.github/skills/backend-tenant-entity-refactor/SKILL.md`](./backend-tenant-entity-refactor/SKILL.md) | Quando o centro da mudanca esta em incluir `tenantId` em entidades, schema e persistencia.   |

## Regra Rapida De Escolha

Se a tarefa for ampla e nao houver um centro dominante claro, comece por `backend-feature-flow`.
Se houver um centro dominante claro, prefira a skill especializada correspondente e use a skill ampla apenas quando a mudanca se espalhar de fato entre as camadas.

### 1. `backend-feature-flow`

Skill principal para criar ou alterar uma feature completa.

Use quando a tarefa envolver uma ou mais destas etapas:

- criar endpoint
- criar ou ajustar DTO
- alterar interface de App
- implementar App
- implementar ou ajustar repositorio
- registrar DI

Essa skill deve orientar o agente a:

1. localizar uma feature equivalente ja existente
2. identificar a camada que realmente controla o comportamento
3. seguir o padrao de nomes, pastas e assinaturas do modulo mais proximo
4. evitar regra de negocio no controller
5. evitar atalho de camada
6. validar com `dotnet build backend/crm.sln` ou build mais estreito

Arquivos auxiliares sugeridos:

- `references/feature-scope.md`
- `references/layer-checklist.md`
- `assets/feature-template.md`

### 2. `backend-endpoint-swagger`

Skill especializada em criar ou alterar endpoints com contrato HTTP consistente.

Use quando a tarefa envolver:

- novo endpoint em controller
- ajuste de rota, verbo, resposta ou autenticacao
- inclusao ou correcao de XML comments
- inclusao ou correcao de `ProducesResponseType`

Essa skill deve forcar o agente a verificar:

1. resumo e parametros documentados
2. status codes alinhados ao codigo real
3. tipo de retorno apropriado com `ActionResult` ou `ActionResult<T>`
4. coerencia entre rota, DTO e retorno
5. preservacao do estilo ja usado em `backend/Api/Controllers`

Arquivos auxiliares sugeridos:

- `references/swagger-checklist.md`
- `assets/controller-doc-template.md`

### 3. `backend-data-access-patterns`

Skill para orientar mudancas em acesso a dados.

Use quando a tarefa envolver:

- novo metodo de repositorio
- decisao entre EF Core e Dapper
- ajuste em stored procedure
- problema de conexao ou mapeamento no repositorio

Essa skill deve orientar o agente a:

1. inspecionar a feature existente antes de escolher EF Core ou Dapper
2. manter repositorio focado em persistencia
3. usar nova conexao por chamada ao trabalhar com Dapper
4. respeitar a assinatura real da stored procedure
5. evitar mover regra de negocio para o repositorio

Arquivos auxiliares sugeridos:

- `references/ef-vs-dapper.md`
- `references/repository-checklist.md`

### 4. `backend-application-validation`

Skill para alteracoes na camada Application com foco em validacao, orquestracao e mapeamento.

Use quando a tarefa envolver:

- validacao de request
- mapeamento entre DTO e entidade
- orquestracao entre repositorios
- padronizacao de excecoes de negocio

Essa skill deve orientar o agente a:

1. validar entradas cedo
2. usar `ArgumentException` e `KeyNotFoundException` conforme o padrao do modulo
3. manter o controller sem regra de negocio
4. mapear DTOs de forma explicita e previsivel
5. reaproveitar DTOs e interfaces existentes antes de criar novos contratos

Arquivos auxiliares sugeridos:

- `references/app-validation-checklist.md`
- `assets/dto-mapping-template.md`

### 5. `backend-auth-jwt-claims`

Skill para tarefas relacionadas a autenticacao e contexto do usuario logado.

Use quando a tarefa envolver:

- endpoints autenticados
- leitura de claims
- geracao ou validacao de JWT
- uso do usuario autenticado na regra de negocio

Essa skill deve orientar o agente a:

1. reutilizar helper de claims existente
2. preservar configuracao JWT em `backend/Api/Program.cs`
3. evitar parse manual de claims em cada controller
4. manter coerencia entre autenticacao, autorizacao e contrato do endpoint

Arquivos auxiliares sugeridos:

- `references/auth-checklist.md`
- `references/jwt-config-notes.md`

## Estrutura Sugerida Para Cada SKILL.md

Cada skill deve ter um `SKILL.md` curto, orientado a descoberta e execucao.

Estrutura recomendada:

```md
---
name: backend-endpoint-swagger
description: "Cria ou atualiza endpoints ASP.NET Core com Swagger completo no Leadfy. Use para rotas, controllers, XML comments, ProducesResponseType, ActionResult e documentacao OpenAPI no backend."
argument-hint: "Descreva o endpoint ou contrato HTTP a ajustar"
user-invocable: true
---

# Backend Endpoint Swagger

## Quando Usar

- Criar endpoint novo
- Corrigir documentacao Swagger
- Ajustar status codes e contrato HTTP

## Antes De Editar

1. Ler o controller alvo e um controller vizinho com padrao semelhante.
2. Confirmar o DTO de entrada e saida.
3. Confirmar se a acao exige autenticacao.

## Procedimento

1. Definir rota, verbo e assinatura.
2. Ajustar DTOs necessarios.
3. Implementar acao fina no controller.
4. Adicionar XML comments e `ProducesResponseType` coerentes.
5. Validar com build.

## Referencias

- [Checklist Swagger](./references/swagger-checklist.md)
- [Template de controller](./assets/controller-doc-template.md)
```

## Como Escrever A Descricao Da Skill

A descricao e o principal gatilho de descoberta.
Ela precisa conter:

- o que a skill faz
- quando usar
- termos reais do projeto
- palavras-chave que o agente provavelmente buscara

Boas palavras-chave para este backend:

- ASP.NET Core
- controller
- Swagger
- XML comments
- DTO
- Application
- Service
- Repository
- Dapper
- EF Core
- JWT
- DI
- Program.cs

Evite descricoes vagas como:

- `Ajuda no backend`
- `Skill para API`
- `Faz varias tarefas`

## O Que Colocar Em `references/`

Use `references/` para conhecimento de apoio que nao precisa ser carregado sempre.

Exemplos adequados:

- checklist por camada
- comparativo EF Core vs Dapper no projeto
- regras para excecoes e retorno HTTP
- mapa de arquivos que costumam ser alterados juntos
- comandos de validacao

## O Que Colocar Em `assets/`

Use `assets/` para material reutilizavel.

Exemplos adequados:

- template de endpoint
- template de DTO
- template de checklist manual
- snippets de frontmatter

## Checklist De Qualidade Antes De Criar A Skill

Antes de considerar a skill pronta, confirme:

1. o nome da pasta e o campo `name` sao identicos
2. a descricao deixa claro quando a skill deve ser usada
3. o `SKILL.md` tem procedimento objetivo e acionavel
4. a skill nao duplica instrucoes globais sem adicionar workflow
5. os arquivos em `references/` e `assets/` sao referenciados com caminho relativo `./`
6. a skill menciona validacao apropriada para o backend

## Ordem Recomendada De Implementacao

Para comecar sem dispersao, implemente nesta ordem:

1. `backend-feature-flow`
2. `backend-endpoint-swagger`
3. `backend-data-access-patterns`
4. `backend-application-validation`
5. `backend-auth-jwt-claims`

Essa sequencia cria primeiro a skill mais ampla, depois cobre contrato HTTP, persistencia, validacao e autenticacao.

## Proximo Passo Pratico

Depois deste guia, o melhor fluxo e:

1. criar a pasta da primeira skill em `.github/skills/backend-feature-flow/`
2. escrever um `SKILL.md` curto com descricao forte e procedimento claro
3. adicionar no maximo 1 ou 2 arquivos em `references/`
4. testar se a skill ficou especifica o suficiente para ser descoberta e usada

Quando essa primeira skill estiver boa, replique o padrao para as demais.

## Prompts Reais Para Testar Descoberta E Uso

Use os prompts abaixo como testes praticos de descoberta e aplicacao das skills.

### `backend-feature-flow`

```text
Quero criar um endpoint para arquivar leads. A mudanca precisa incluir rota nova no controller, metodo novo na interface e no LeadApp, ajuste no repositorio e registro de DI se necessario. Siga o padrao existente da feature de leads e valide com build.
```

```text
Preciso adicionar um novo fluxo de reativacao de oportunidade com endpoint, validacao na Application, persistencia no Repository e ajuste do contrato de resposta. Reuse os padroes existentes no modulo de oportunidades.
```

### `backend-endpoint-swagger`

```text
Crie um endpoint GET para buscar produto por id no ProductController com XML comments completos, ProducesResponseType coerentes e retorno ActionResult<ProductResponse>, seguindo o estilo atual dos controllers.
```

```text
Revise o endpoint PATCH de desativacao de leads para alinhar rota, status codes e documentacao Swagger com o comportamento real do metodo.
```

### `backend-data-access-patterns`

```text
Preciso adicionar um metodo no OpportunityRepo para buscar oportunidades por usuario. Antes de implementar, verifique se o modulo deve usar EF Core ou Dapper e mantenha o padrao existente da feature.
```

```text
Analise o update de oportunidades e ajuste o metodo de repository para respeitar exatamente a assinatura da stored procedure, sem parametros extras, mantendo nova conexao por chamada no Dapper.
```

### `backend-application-validation`

```text
Revise o LeadApp para centralizar validacoes de entrada, manter os mapeamentos explicitos e padronizar ArgumentException e KeyNotFoundException conforme o estilo do modulo.
```

```text
Preciso adicionar validacao de negocio na Application para impedir atualizacao de produto com preco menor ou igual a zero, mantendo o controller fino e o repository sem regra de negocio.
```

### `backend-auth-jwt-claims`

```text
Crie um endpoint autenticado que use o usuario logado para registrar uma interacao. Reaproveite o helper de claims existente, preserve o uso de Authorize e nao faca parse manual do ClaimsPrincipal no controller.
```

```text
Revise o fluxo de autenticacao para confirmar se o TokenService esta emitindo as claims necessarias para os controllers que usam User.GetAuthenticatedUserId(), sem alterar a configuracao JWT alem do necessario.
```
