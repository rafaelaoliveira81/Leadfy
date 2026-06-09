# Template De Implementacao Da Feature

Use este roteiro como checklist de execucao ao criar ou expandir uma feature backend.

## 1. Definicao

- entidade ou modulo afetado:
- objetivo da feature:
- endpoint novo ou alterado:
- autenticacao necessaria:

## 2. Contrato HTTP

- controller alvo:
- rota:
- verbo HTTP:
- request DTO:
- response DTO:
- status codes esperados:

## 3. Application

- interface de App a alterar:
- metodo novo ou alterado:
- validacoes necessarias:
- mapeamentos necessarios:
- repositorios ou services envolvidos:

## 4. Persistencia

- repository alvo:
- metodo novo ou alterado:
- padrao usado: EF Core ou Dapper:
- consulta, procedure ou entidade envolvida:

## 5. Infraestrutura

- DI a registrar em `backend/Api/Program.cs`:
- helper existente a reaproveitar:

## 6. Validacao

- teste estreito disponivel:
- comando de build:
- resultado esperado:
