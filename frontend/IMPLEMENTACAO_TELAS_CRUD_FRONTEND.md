# Implementacao de Telas CRUD no Frontend

Este documento registra o padrao usado na implementacao da tela de Produtos para servir como base na criacao de outras telas como Leads, Owners e Usuarios.

## Objetivo

Padronizar a implementacao de telas CRUD no frontend com:

- separacao de responsabilidades
- reutilizacao de componentes
- baixo acoplamento com a entidade
- aderencia ao layout atual do projeto
- uso do servico de API ja existente
- manutencao simples para novas features

## Stack Real do Projeto

A implementacao foi feita considerando o estado atual do repositorio:

- React com JavaScript
- CSS Modules
- React Router
- Axios via servicos em `frontend/src/services`
- `react-hook-form` para formularios
- `zod` + `@hookform/resolvers` para validacao
- componentes compartilhados criados manualmente, sem biblioteca de UI externa

Importante:

- o projeto nao esta em TypeScript
- a feature de Products foi montada sem alterar o backend
- quando a API nao suporta paginação ou combinacao de filtros, a derivacao pode ser feita no cliente

## Estrutura Recomendada

A estrutura criada para Products deve virar referencia para novas telas:

```text
frontend/src/
  layouts/
    AppLayout/
  components/
    Sidebar/
    Topbar/
    ui/
      Button/
      DataTable/
      EmptyState/
      LoadingState/
      Modal/
      Pagination/
      StatusBadge/
      Toast/
  features/
    products/
      components/
      constants/
      hooks/
      mappers/
      schemas/
  pages/
    Home/
    Products/
  services/
```

## Responsabilidade de Cada Camada

### 1. `layouts`

Responsavel pelo esqueleto visual da aplicacao.

Exemplo:

- `AppLayout` concentra Sidebar, Topbar e a area principal de conteudo

Regra:

- nao colocar regra de negocio aqui
- o layout deve apenas compor a estrutura visual comum

### 2. `components/ui`

Responsavel por componentes reutilizaveis e genericos.

Componentes criados:

- `Button`
- `Modal`
- `DataTable`
- `Pagination`
- `StatusBadge`
- `LoadingState`
- `EmptyState`
- `ToastProvider` + `useToast`

Regra:

- esses componentes nao conhecem a entidade
- devem receber tudo por props
- devem poder ser reaproveitados em outras telas sem mudanca estrutural

### 3. `features/<entidade>`

Responsavel pela regra da entidade.

Para Products foram criados:

- `components`: partes visuais especificas da entidade
- `constants`: opcoes e valores padrao
- `hooks`: orquestracao da tela e do formulario
- `mappers`: formatacao e normalizacao de dados
- `schemas`: validacao do formulario

Regra:

- a regra da entidade deve ficar aqui
- nao colocar fetch, mutacoes e regras de filtro dentro da pagina diretamente

### 4. `pages`

Responsavel por montar a tela final.

Exemplo:

- `Products.js` usa o layout, chama o hook principal da feature e conecta os componentes visuais

Regra:

- a pagina deve orquestrar, nao concentrar regra
- a pagina conecta layout + hook + componentes

### 5. `services`

Responsavel pela comunicacao HTTP.

Regra:

- continuar usando os servicos centralizados em `frontend/src/services`
- evitar duplicar client HTTP dentro da feature
- a feature consome o servico, nao recria o acesso HTTP

## Padrao Usado na Tela de Products

### Fluxo principal

1. A pagina `Products` entra no `AppLayout`.
2. O hook `useProductsPage` carrega a lista da API.
3. O hook deriva os dados para:
   - busca por nome
   - filtro por status
   - paginacao
4. A pagina monta:
   - cabecalho da entidade
   - barra de filtros
   - tabela
   - paginacao
   - modal de create/edit
5. Mutacoes chamam o servico da API e recarregam a lista.
6. Feedback visual e feito com toast e estados de loading/erro.

## Arquivos-Chave da Implementacao

### Base compartilhada

- `frontend/src/layouts/AppLayout/AppLayout.js`
- `frontend/src/components/ui/Button/Button.js`
- `frontend/src/components/ui/Modal/Modal.js`
- `frontend/src/components/ui/DataTable/DataTable.js`
- `frontend/src/components/ui/Pagination/Pagination.js`
- `frontend/src/components/ui/Toast/Toast.js`

### Feature Products

- `frontend/src/features/products/hooks/useProductsPage.js`
- `frontend/src/features/products/hooks/useProductForm.js`
- `frontend/src/features/products/components/ProductFilters.js`
- `frontend/src/features/products/components/ProductTable.js`
- `frontend/src/features/products/components/ProductRowActions.js`
- `frontend/src/features/products/components/ProductForm.js`
- `frontend/src/features/products/components/ProductFormModal.js`

### Pagina

- `frontend/src/pages/Products/Products.js`

## Como Repetir Esse Padrao em Outra Entidade

Exemplo: criar uma tela de Owners.

### 1. Criar o servico da entidade se ainda nao existir

Exemplo:

- `frontend/src/services/owner.js`

Responsabilidade:

- listar
- buscar por id
- criar
- editar
- ativar/desativar se existir

Seguir o mesmo formato de tratamento de erro usado em `product.js`.

### 2. Criar a feature isolada

Estrutura sugerida:

```text
frontend/src/features/owners/
  components/
  constants/
  hooks/
  mappers/
  schemas/
```

### 3. Criar um hook principal da tela

Exemplo:

- `useOwnersPage`

Esse hook deve concentrar:

- fetch inicial
- loading
- erro
- busca
- filtros
- paginacao
- estado do modal
- mutacoes

Regra:

- a pagina nao deve carregar esse peso
- o hook deve devolver uma interface simples para a tela montar a UI

### 4. Criar um hook do formulario

Exemplo:

- `useOwnerForm`

Esse hook deve concentrar:

- validacao
- valores iniciais
- normalizacao de campos
- submit de create/edit

Regra:

- se houver campo monetario, telefone, documento ou datas, a formatacao deve ficar em mapper/hook, nao no componente visual

### 5. Criar componentes especificos da entidade

Padrao sugerido:

- `<Entidade>ListHeader`
- `<Entidade>Filters`
- `<Entidade>Table`
- `<Entidade>RowActions`
- `<Entidade>Form`
- `<Entidade>FormModal`

Regra:

- os componentes da feature configuram os componentes genericos
- exemplo: a tabela da entidade configura colunas sobre o `DataTable`

### 6. Criar a pagina

Exemplo:

- `frontend/src/pages/Owners/Owners.js`

Responsabilidade:

- usar `AppLayout`
- usar `useToast`
- consumir o hook principal da feature
- montar a composicao final da tela

### 7. Registrar a rota

Atualizar:

- `frontend/src/App.js`

## Checklist de Implementacao para Novas Telas

Antes de comecar:

- existe servico HTTP da entidade?
- existe rota definida no menu?
- a API suporta os filtros e paginacao esperados?

Durante a implementacao:

- extrair regra da pagina para hooks
- manter componentes visuais sem regra de negocio pesada
- reutilizar `AppLayout`
- reutilizar `Modal`, `DataTable`, `Pagination`, `Button`, `Toast`
- criar validacao com `zod`
- usar `react-hook-form`
- manter mensagens de erro e sucesso consistentes

Ao finalizar:

- validar listagem
- validar busca
- validar filtros
- validar paginação
- validar create em modal
- validar edit em modal
- validar acao por linha
- validar empty state
- validar loading state
- validar build do frontend

## Regras de Decisao que Devem Ser Mantidas

### 1. A pagina nao deve concentrar a regra

Errado:

- fazer fetch na pagina
- calcular filtros na pagina
- controlar form e submit na pagina

Certo:

- pagina monta a tela
- hook da feature concentra a logica

### 2. Componentes genericos nao devem conhecer a entidade

Errado:

- `DataTable` saber o que e produto
- `Modal` saber o que e owner

Certo:

- tudo entra por props
- a especializacao acontece dentro da feature

### 3. O formulario nao deve conter regra de status quando isso for responsabilidade da listagem

No caso de Products:

- `isActive` nao aparece em create/edit
- mudanca de status so acontece pela area de acoes da tabela

Essa mesma regra deve ser avaliada para outras entidades com soft delete ou ativacao.

### 4. O servico continua sendo a fronteira HTTP

Regra:

- hooks nao devem montar axios diretamente
- hooks chamam os servicos em `frontend/src/services`

## Limitacoes e Como Decidir

A implementacao atual de Products precisou respeitar o backend existente.

Isso gerou duas decisoes importantes:

### Quando a API nao suporta combinacao de filtros

Exemplo:

- busca por nome e status juntos

Solucao aplicada:

- carregar a colecao base
- aplicar filtros no cliente

### Quando a API nao suporta paginacao

Solucao aplicada:

- paginacao client-side

Quando mudar:

- se o volume de dados crescer
- se a API passar a suportar `page`, `pageSize`, `total` e filtros compostos

Nesse caso, o ideal e mover a paginacao para o servidor e manter a mesma composicao visual da tela.

## Convencoes de Nome

Para manter consistencia, usar:

- `use<Entity>Page`
- `use<Entity>Form`
- `<Entity>Filters`
- `<Entity>Table`
- `<Entity>Form`
- `<Entity>FormModal`
- `<Entity>RowActions`
- `<entity>.schema.js`
- `<entity>.constants.js`
- `<entity>.mapper.js`

Exemplos:

- `useUsersPage`
- `UserTable`
- `user.schema.js`

## Convencoes de Estilo

- cada componente com seu proprio CSS Module
- manter a paleta ja existente do projeto
- evitar estilos inline, exceto quando forem pontuais e pequenos
- manter acessibilidade minima:
  - `aria-label`
  - `aria-expanded`
  - `role="dialog"`
  - `aria-modal`
  - `aria-live` para toast

## Passo a Passo Minimo para Nova Tela

1. Criar ou revisar o servico da entidade.
2. Criar a pasta da feature.
3. Criar constants, schema e mapper.
4. Criar hook principal da tela.
5. Criar hook do formulario.
6. Criar componentes da feature.
7. Criar a pagina com `AppLayout`.
8. Registrar a rota.
9. Validar os fluxos.
10. Rodar `build`.

## O que vale reaproveitar imediatamente

As proximas telas devem tentar reutilizar primeiro:

- `AppLayout`
- `Button`
- `Modal`
- `DataTable`
- `Pagination`
- `StatusBadge`
- `EmptyState`
- `LoadingState`
- `ToastProvider` e `useToast`

Depois disso, criar apenas o que for especifico da entidade.

## Resultado Esperado

Se esse padrao for seguido, novas telas CRUD tendem a ficar:

- mais rapidas de implementar
- mais consistentes visualmente
- mais simples de manter
- com menos duplicacao
- com menor risco de colocar regra de negocio dentro do componente errado
