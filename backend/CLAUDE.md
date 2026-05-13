# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Backend de um CRM de vendas (Crm-Vendas) construído em **.NET 6 / ASP.NET Core** com **Clean Architecture** em quatro camadas. O banco de dados é **SQL Server** via **Entity Framework Core 6**.

---

## Commands

### Build

```bash
dotnet build crm.sln
```

### Run (API)

```bash
dotnet run --project Api
```

Swagger disponível em `https://localhost:7287/swagger`. CORS liberado para `http://localhost:3000`.

### Database migrations

```bash
# Executar a partir da pasta Repository (contexto do DbContext)
dotnet ef database update --project Repository --startup-project Api

# Criar nova migration
dotnet ef migrations add <NomeDaMigration> --project Repository --startup-project Api
```

### Connection string de desenvolvimento

`appsettings.Development.json` já contém as credenciais locais:

```
Server=localhost,1433;Database=CrmVendasDb;User Id=sa;Password=SenhaForte@123;TrustServerCertificate=True;
```

Requer instância SQL Server local (ou Docker) na porta 1433.

---

## Architecture

O projeto segue Clean Architecture com dependências apontando sempre para dentro:

```
Api  →  Application  →  Domain
         ↓
     Repository  (depende de Domain)
Service  (depende de Domain)
```

### Domain

Entidades de domínio puras sem dependências externas.

- **Entities**: `User`, `Lead`, `Owner`, `Product`, `Opportunity`, `Interaction`
- **Enuns/** (namespace correto é `Domain.Enuns`): `UserRole`, `OpportunityStage`, `CrmEntityType`
- **Interfaces**: `IPasswordHasher` (contrato de hashing implementado em Application)

Cada entidade define construtores que inicializam `IsActive = true` e `CreatedAt = DateTime.UtcNow`, e métodos `Activate()` / `Deactivate()`.

### Application

Orquestra os casos de uso. Cada entidade tem uma interface `IXxxApp` e uma implementação `XxxApp`.

- Valida entradas antes de ir ao banco
- Delega persistência ao `Repository`
- `PasswordHasher` implementa `IPasswordHasher` do Domain

### Repository

Acesso a dados com EF Core.

- `CRMContext` (`Repository/Context/CRMContext.cs`) — DbContext com todos os DbSets
- `BaseRepo<T>` — repositório genérico com operações CRUD assíncronas
- Repositórios concretos: `UserRepository`, `OwnerRepo`, `LeadRepo`, `ProductRepo`, `OpportunityRepo`, `InteractionRepo`
- Configurações Fluent API em `Repository/Configurations/`
- Migrations em `Repository/Migrations/`

### Api

Camada de apresentação; controllers mapeiam HTTP ↔ Application.

- Controllers seguem padrão: injetam `IXxxApp`, retornam `ActionResult` com try/catch para `ArgumentException` (400), `KeyNotFoundException` (404), `UnauthorizedAccessException` (401)
- Todos os métodos públicos têm comentários XML (`/// <summary>`) para geração automática do Swagger
- DTOs em `Api/Models/` separados por entidade (pastas `User/`, `Lead/`, etc.)
- `Program.cs` registra todos os `IXxxApp`, repos e `IAiService` com `AddScoped`

### Service

Integrações externas.

- `AiService` consome a **GitHub Models API** (modelo `gpt-4.1`) configurada via `appsettings`:
  - `GitHubModels:BaseUrl`
  - `GitHubModels:Token`

---

## Key Domain Concepts

| Entidade      | Descrição                                                                                             |
| ------------- | ----------------------------------------------------------------------------------------------------- |
| `User`        | Usuário do sistema com `UserRole` (Admin, Manager, SalesRepresentative, CustomerSupport, RegularUser) |
| `Owner`       | Associa um usuário a uma entidade do CRM (ownership)                                                  |
| `Lead`        | Cliente potencial                                                                                     |
| `Product`     | Produto ou serviço ofertado                                                                           |
| `Opportunity` | Negociação vinculada a Lead e Product com `OpportunityStage` (NewLead → Won/Lost)                     |
| `Interaction` | Registro de atividades/contatos relacionados a entidades                                              |

`OpportunityStage` pipeline: `NewLead → Contacted → Qualified → ProposalSent → Negotiation → Won / Lost`

---

## Conventions

- Nomes de classes, propriedades e métodos em **PascalCase** (inglês)
- Mensagens de erro e comentários em **português**
- Async/await em todos os métodos de repositório e application
- Não há autenticação JWT implementada ainda — `UseAuthorization()` está registrado mas sem política configurada
