# Leadfy Project Guidelines

## Project Shape

Leadfy is a monorepo with two main applications:

- `backend/`: ASP.NET Core 6 REST API split into `Api`, `Application`, `Domain`, `Repository` and `Service`.
- `crm-app/`: React application that consumes the API.

Prefer matching the patterns already present in the touched module instead of introducing a new structure or naming convention.

## How To Work In This Repo

- Keep changes small, local and consistent with the current layer or feature.
- Reuse existing DTOs, services, helpers and UI patterns before creating new abstractions.
- Preserve Portuguese naming and user-facing text when the surrounding feature already uses Portuguese.
- When updating docs or examples, keep them aligned with the current repository structure and actual commands in `README.md`.

## Validation

- For backend changes, prefer validating with `dotnet build backend/crm.sln` or a narrower command for the touched project.
- For frontend changes, prefer validating with `npm test -- --watch=false` or `npm run build` inside `crm-app` when relevant.
- If a task affects only instruction or documentation files, validate by checking the instruction split, frontmatter and consistency with the codebase.

## Scoped Instructions

Detailed implementation rules live in scoped instruction files:

- `.github/instructions/backend.instructions.md`
- `.github/instructions/frontend.instructions.md`
- `.github/instructions/swagger.instructions.md`

Use those rules when the task touches the corresponding area.
