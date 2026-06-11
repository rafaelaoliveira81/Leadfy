---
description: "Use when editing the ASP.NET Core backend, including API, Application, Domain, Repository, Service, controllers, DTOs, dependency injection, Swagger, EF Core, Dapper, and appsettings."
name: "Leadfy Backend Instructions"
applyTo: "backend/**"
---

# Leadfy Backend Instructions

## Architecture

- Respect the layer flow: `Api -> Application -> Service -> Repository`, with `Domain` shared by the business layers.
- `Api` must depend on `Application`, not directly on `Repository` or `Domain` implementations.
- Keep business rules in `Application` and `Service`; keep persistence concerns in `Repository`.
- `Domain` should contain entities, enums, value objects and contracts, not transport or controller concerns.

## API And Controllers

- Keep controllers thin: receive the request, delegate to the application layer, and translate the result to HTTP.
- Follow the current controller style already present in `backend/Api/Controllers`, including `ActionResult`, `ProducesResponseType` and XML documentation.
- New or changed endpoints should include XML comments with summary, parameter descriptions and response codes so Swagger stays complete.
- Reuse helpers such as authenticated user resolution instead of reimplementing claim parsing in each controller.

## Application, Service And Repository

- Put orchestration and DTO mapping in `Application`.
- Put business validation and domain decisions in `Service` when the rule is not just request orchestration.
- Keep repositories focused on data access only; always apply tenant filtering to prevent data leakage.
- Reuse existing DTO folders in `backend/Application/DTO` and existing interfaces before creating new contracts.
- **For tenant-scoped operations**: inject `ITenantProvider`, extract the authenticated tenant via `GetRequiredTenantId()`, and propagate it through all repository and service calls.
- Never allow a repository method on a tenant-scoped entity to execute without an explicit `tenantId` parameter.

## Data Access And Configuration

- The project uses SQL Server, Entity Framework Core and Dapper with a **multi-tenant architecture** (single database, single schema).
- All tenant-scoped entities must follow the multi-tenancy pattern described below; match the existing access style of the touched feature.
- Always filter queries by `TenantId` to prevent cross-tenant data leakage; use query filters in configurations and explicit checks in stored procedures.
- Keep the `DefaultConnection` configuration aligned with `backend/Api/appsettings.json` and `appsettings.Development.json`.
- Preserve the current DI style in `backend/Api/Program.cs`: register interfaces and concrete implementations explicitly.
- When touching startup or infrastructure, avoid changing CORS, JWT or Swagger setup unless the task requires it.

## Multi-Tenancy (Single Database, Single Schema)

Leadfy uses a multi-tenant architecture where all tenants share a single database and schema. Data isolation is enforced at the application layer through `TenantId` filtering.

### Tenant-Scoped Entities

Entities that belong to a specific tenant must include a `TenantId` property and follow this pattern:

- **Domain**: Add `public Guid TenantId { get; set; }` property and configure a foreign key relationship to `Tenant` in the entity configuration.
- **Entity Configuration** (in `Repository/Configurations`):
  - Mark `TenantId` as required: `.Property(e => e.TenantId).IsRequired()`
  - Add an index: `.HasIndex(e => e.TenantId)`
  - For data isolation: `.HasQueryFilter(e => !_tenantProvider.CurrentTenantId.HasValue || e.TenantId == _tenantProvider.CurrentTenantId.Value)`
  - Configure FK to `Tenant` with `OnDelete(DeleteBehavior.Restrict)`
- **Repository Interface**: Add `tenantId` parameter to query methods (e.g., `GetPagedAsync(int page, int pageSize, Guid tenantId)`)
- **Repository Implementation**: Pass `tenantId` to stored procedures and EF Core queries; always filter by tenant to prevent cross-tenant data leakage
- **Application Layer**: Inject `ITenantProvider` and call `GetRequiredTenantId()` to extract the authenticated tenant from claims; pass it to all repository methods
- **Controllers**: Use `ITenantProvider` or extract from claims via `UserClaimsHelper` to ensure tenant isolation in responses

**Existing examples**: `User`, `Product`, `Prompt` demonstrate the full pattern.

### Tenant Provider And Claims

- Use `ITenantProvider` service (injected in `Application`) to access `CurrentTenantId` and call `GetRequiredTenantId()` for the authenticated tenant.
- When an endpoint is protected by `[Authorize]`, always propagate `tenantId` through the request chain to ensure isolation.
- Store `TenantId` in JWT claims and extract it consistently across all operations.

### Data Isolation Rules

- **All queries** on tenant-scoped entities must include an explicit or implicit (via query filter) `TenantId == CurrentTenantId` check.
- **No exceptions**: never retrieve or modify tenant data without validating tenant ownership.
- **Stored procedures**: include `@TenantId` parameter and filter at the SQL layer for safety.

## Conventions

- Prefer descriptive names in English for code symbols, but keep Portuguese messages and comments when the surrounding code already uses them.
- Avoid business rules inside controllers and repositories.
- Avoid cross-layer shortcuts just to reduce code volume.
- Keep methods focused and follow existing null/error handling patterns in the touched slice.

## Validation

- Prefer `dotnet build backend/crm.sln` after backend changes.
- If the change is isolated, a narrower build for the touched project is acceptable.
