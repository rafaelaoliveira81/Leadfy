# Escopo Da Feature Backend

Use este roteiro para decidir o que realmente precisa mudar antes de editar o backend.

## Perguntas De Escopo

1. A feature altera apenas o contrato HTTP ou tambem a regra de negocio?
2. A feature exige novo DTO ou um DTO existente ja cobre o caso?
3. A feature precisa de novo metodo em interface de App ou Repository?
4. A feature depende de autenticacao, claims ou contexto do usuario logado?
5. A feature deve seguir um padrao existente em outra entidade?

## Sinais De Que A Mudanca E Multi-Camada

- existe endpoint novo ou mudanca de rota
- o request ou response precisa mudar
- a Application precisa validar ou orquestrar novos passos
- o Repository precisa de novo metodo ou ajuste de consulta
- a DI precisa registrar novo contrato

## Ordem Recomendada De Analise

1. Controller ou ponto de entrada equivalente
2. Interface e implementacao de App
3. Service, se a regra de negocio estiver ali
4. Interface e implementacao de Repository
5. DTOs envolvidos
6. Registro de DI em `backend/Api/Program.cs`

## Regra Pratica

Se dois ou mais itens acima precisarem mudar juntos, trate a tarefa como feature-flow e mantenha a implementacao coordenada entre as camadas.
