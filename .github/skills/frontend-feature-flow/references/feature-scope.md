# Escopo Da Feature Frontend

Use este roteiro para decidir o que realmente precisa mudar antes de editar o frontend.

## Perguntas De Escopo

1. A tarefa muda apenas a UI ou tambem o service e o payload da API?
2. A tela exige rota nova, protecao por autenticacao ou contexto?
3. Ja existe uma page equivalente para copiar o padrao?
4. A mudanca exige utilitario de normalizacao ou formatacao?
5. O fluxo e uma tela CRUD repetida ou um fluxo especial como Kanban?

## Sinais De Que A Mudanca E Multi-Arquivo

- page e service mudam juntos
- modal, formulario e listagem mudam no mesmo fluxo
- rota e contexto autenticado fazem parte da entrega
- ha necessidade de preservar payload esperado pelo backend
- utilitarios e CSS Module precisam mudar junto com a tela

## Ordem Recomendada De Analise

1. Page ou fluxo equivalente
2. Service relacionado
3. Route ou contexto, se aplicavel
4. Utils relacionados
5. CSS Module e componentes reutilizados

## Regra Pratica

Se dois ou mais desses pontos precisarem mudar juntos, trate a tarefa como feature-flow do frontend.
