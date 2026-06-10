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
- Keep repositories focused on data access only.
- Reuse existing DTO folders in `backend/Application/DTO` and existing interfaces before creating new contracts.

## Data Access And Configuration

- The project uses SQL Server, Entity Framework Core and Dapper; match the existing access style of the touched feature.
- Keep the `DefaultConnection` configuration aligned with `backend/Api/appsettings.json` and `appsettings.Development.json`.
- Preserve the current DI style in `backend/Api/Program.cs`: register interfaces and concrete implementations explicitly.
- When touching startup or infrastructure, avoid changing CORS, JWT or Swagger setup unless the task requires it.

## Conventions

- Prefer descriptive names in English for code symbols, but keep Portuguese messages and comments when the surrounding code already uses them.
- Avoid business rules inside controllers and repositories.
- Avoid cross-layer shortcuts just to reduce code volume.
- Keep methods focused and follow existing null/error handling patterns in the touched slice.

## Validation

- Prefer `dotnet build backend/crm.sln` after backend changes.
- If the change is isolated, a narrower build for the touched project is acceptable.
