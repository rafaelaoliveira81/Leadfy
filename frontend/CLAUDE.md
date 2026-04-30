# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
npm start      # dev server at http://localhost:3000
npm run build  # production build
npm test       # run tests (Jest + React Testing Library)
```

The backend API runs at `https://localhost:7287/api` (configured in `src/services/client.js`).

## Architecture

React (JavaScript, not TypeScript) with Create React App. CSS Modules for styling. No Redux or global state — all state lives in feature hooks.

### Layer responsibilities

```
src/
  layouts/AppLayout/       # Shell: Sidebar + Topbar + content area. No business logic.
  components/ui/           # Generic, entity-agnostic components (Button, Modal, DataTable,
                           # Pagination, StatusBadge, LoadingState, EmptyState, Toast)
  features/<entity>/       # Entity-specific logic
    components/            # UI components for this entity only
    constants/             # Page size, debounce values, dropdown options
    hooks/                 # All state, fetching, filtering, pagination, mutations
    mappers/               # Data transformation between API shape and UI shape
    schemas/               # Zod validation schemas used with react-hook-form
  pages/<Entity>/          # Thin orchestration layer: imports hook + components, no logic
  services/                # HTTP layer via Axios (one file per entity)
```

### Data flow (CRUD screen pattern)

1. `pages/<Entity>` calls the main feature hook (e.g. `useProductsPage`)
2. Hook fetches from the API service, derives filtered/paginated data client-side, manages modal state and mutations
3. Page composes feature components (`<Entity>ListHeader`, `<Entity>Filters`, `<Entity>Table`, `Pagination`, `<Entity>FormModal`)
4. Mutations reload the full list; feedback goes through `useToast()`

### Rules

- Business logic, filtering, and mutations belong in feature hooks — never in page components
- `components/ui/` components receive everything via props; they have no knowledge of entities
- Services in `src/services/` are the only place that touches Axios; features call services, never recreate HTTP access
- When the backend doesn't support combined filters or pagination, derive them client-side in the hook (see `useProductsPage` as reference)
- The Products feature (`src/features/products/`) is the canonical reference implementation for all other entities

### Current features

| Route | Feature |
|-------|---------|
| `/` | Home |
| `/products` | Products CRUD |
| `/leads` | Leads CRUD |
| `/owners` | Owers (Owners) CRUD |
| `/users` | Users CRUD |
| `/opportunities` | Opportunities list |
| `/opportunities/kanban` | Opportunities Kanban (drag-and-drop via @dnd-kit) |

The Kanban feature lives under `src/features/opportunities/kanban/` with its own components, hooks, and constants. `KanbanProgress` is currently commented out.

### API service pattern

Each service file exports functions like `Create`, `GetAll`, `GetById`, `Update`, `Delete`, `Deactivate`, `Activate`. Errors are normalized to `{ type: 'network' | 'validation' | 'not_found' | 'server' | 'unknown', message, errors? }`.

### Forms

React Hook Form + Zod schemas (`@hookform/resolvers/zod`). Each entity has `use<Entity>Form` hook and a Zod schema in `features/<entity>/schemas/`.

### Toast notifications

Wrap the app with `ToastProvider` (already in `App.js`). Use `useToast()` anywhere to call `showToast({ type, message })`.
