---
name: frontend-feature-flow
description: "Cria ou altera uma feature completa no frontend React do Leadfy. Use quando precisar ajustar page, component, service, route, context, utils e CSS Modules dentro do crm-app/src seguindo os padroes existentes do projeto."
argument-hint: "Descreva a feature frontend, tela ou fluxo a implementar"
user-invocable: true
---

# Frontend Feature Flow

Use esta skill quando a tarefa exigir alteracoes coordenadas em mais de uma parte do `crm-app/src`.
Ela e a skill guarda-chuva do frontend e deve ser usada quando a entrega cruza page, component, service, route, context, utils ou estilo sem um unico centro dominante.

## Papel Desta Skill No Conjunto

Use esta skill quando a mudanca atravessar varias partes do frontend e nao houver um unico ponto dominante claro.
Se o centro da mudanca estiver nitidamente em tela CRUD, integracao de service, autenticacao/rotas ou Kanban, prefira a skill especializada correspondente.

## Quando Usar

- Criar uma nova feature frontend do inicio ao fim
- Expandir uma feature existente em varias partes do `crm-app/src`
- Ajustar pagina, service e route na mesma entrega
- Implementar um fluxo que exige normalizacao em `utils` e renderizacao em page/component
- Organizar uma mudanca hoje espalhada entre tela, service e contexto

## Nao Use Quando

- A tarefa estiver concentrada em uma unica tela CRUD no padrao repetido do projeto
- A mudanca principal estiver isolada em `services`
- O foco principal for `AuthContext` ou guardas de rota
- O centro da mudanca for o Kanban

## Antes De Editar

1. Localize a tela ou fluxo equivalente mais proximo.
2. Identifique onde o comportamento realmente e controlado: page, component, service, route, context ou util.
3. Confirme quais arquivos precisam mudar juntos para entregar a feature.
4. Reaproveite componentes, services, utils e estilos existentes antes de criar novos artefatos.

Consulte:

- [Escopo da feature](./references/feature-scope.md)
- [Checklist por camada frontend](./references/layer-checklist.md)
- [Template de implementacao](./assets/feature-template.md)

## Procedimento

1. Identifique o ponto de entrada da feature.

Comece pela page, rota ou component equivalente no mesmo dominio.
Se o arquivo inicial apenas encaminha comportamento, pule para onde o estado, o fetch ou a composicao sao realmente decididos.

2. Defina o fluxo da feature.

Confirme page, route, service, contexto, payloads, componentes reutilizados e estilo envolvidos.

3. Ajuste a tela mantendo o padrao local.

Mantenha a estrutura do modulo proximo: page em `pages/Feature/Feature.js`, componente em `components/Component/Component.js` e CSS Module no arquivo underscore-prefixed correspondente.

4. Atualize integracao e utilitarios sem misturar responsabilidades.

Deixe HTTP em `services`, normalizacao em `utils` quando o padrao ja existir, e composicao de UI na page ou component.

5. Revise autenticacao e rotas quando fizerem parte da entrega.

Se a feature depender de rota protegida, contexto autenticado ou claims do token, mantenha o comportamento alinhado ao `AuthContext` e aos guardas existentes.

6. Valide a mudanca.

Prefira validacao estreita quando existir.
Caso contrario, rode `npm run build` em `crm-app`.

## Regras Operacionais Deste Projeto

- Preserve o padrao de pastas, nomes e imports ja usado no modulo
- Evite criar abstrações novas sem equivalente proximo no projeto
- Mantenha chamadas de API em `services`
- Mantenha normalizacao e formatacao em `utils` quando esse padrao ja existir
- Preserve toasts, modais, loading states e CSS Modules no estilo das telas vizinhas

## Resultado Esperado

- feature consistente entre page, service, route e utils
- responsabilidades bem separadas no frontend
- padrao visual e estrutural preservado
- validacao executada apos a edicao

## Referencias

- [Escopo da feature](./references/feature-scope.md)
- [Checklist por camada frontend](./references/layer-checklist.md)
- [Template de implementacao](./assets/feature-template.md)
