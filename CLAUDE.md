# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Monorepo Structure

This is a full-stack sales CRM (Crm-Vendas) with a **React frontend** and a **.NET 6 backend** in the same repository.

- `backend/` — ASP.NET Core API with Clean Architecture. See `backend/CLAUDE.md` for commands and layer details.
- `frontend/` — React (JavaScript, not TypeScript) SPA with CSS Modules. See `frontend/CLAUDE.md` for commands and patterns.
- `frontend/IMPLEMENTACAO_TELAS_CRUD_FRONTEND.md` — step-by-step guide for implementing new CRUD screens; follow it when adding a new entity.

## Local Development Setup

The root `.env` contains SQL Server credentials used by a local instance (or Docker container) on port 1433:

```
Server=localhost,1433;Database=CrmVendasDb;User Id=sa;Password=SenhaForte@123;TrustServerCertificate=True;
```

Start the stack:

1. Ensure SQL Server is running on port 1433
2. `cd backend && dotnet run --project Api` → API at `https://localhost:7287`
3. `cd frontend && npm start` → App at `http://localhost:3000`

CORS is configured in `backend/Api/Program.cs` to allow only `http://localhost:3000`. The frontend Axios client points to `https://localhost:7287/api` (configured in `frontend/src/services/client.js`).

## Architecture Overview

### Backend — Clean Architecture

```
Api  →  Application  →  Domain
         ↓
     Repository  (persists Domain entities via EF Core + SQL Server)
     Service     (external integrations, e.g. GitHub Models API)
```

All dependencies point inward toward Domain. `Program.cs` wires up every `IXxxApp`, repository, and `IAiService` as `AddScoped`.

### Frontend — Feature-Hook Pattern

```
pages/<Entity>        ← thin orchestration, no logic
  └── useXxxPage      ← all state, fetching, filtering, pagination, mutations
       ├── services/  ← Axios HTTP layer (one file per entity)
       └── components ← entity-specific UI (receive data via props)
            └── components/ui/ ← generic, entity-agnostic components
```

The Products feature (`frontend/src/features/products/`) is the canonical reference implementation.

## Domain Entities

| Entity        | Description                                                                                     |
| ------------- | ----------------------------------------------------------------------------------------------- |
| `User`        | System user with `UserRole` (Admin, Manager, SalesRepresentative, CustomerSupport, RegularUser) |
| `Owner`       | Associates a user to a CRM entity (ownership)                                                   |
| `Lead`        | Prospective customer                                                                            |
| `Product`     | Product or service offered                                                                      |
| `Opportunity` | Sales negotiation linked to a Lead and Product, tracked through `OpportunityStage`              |
| `Interaction` | Activity/contact record tied to any CRM entity                                                  |

`OpportunityStage` pipeline: `NewLead → Contacted → Qualified → ProposalSent → Negotiation → Won / Lost`

## Cross-Cutting Conventions

- Code identifiers are in **English** (PascalCase for C#, camelCase for JS); error messages and comments are in **Portuguese**
- No JWT authentication yet — `UseAuthorization()` is registered in `Program.cs` but no policy is configured
- The AI feature uses the **GitHub Models API** (`gpt-4.1`) via `backend/Service/Services/AiService.cs`; credentials are in `appsettings` under `GitHubModels:BaseUrl` and `GitHubModels:Token`
