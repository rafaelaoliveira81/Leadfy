---
description: "Use when editing the React frontend, including pages, components, context, routes, services, utils, CSS Modules, forms, modals, tables, Kanban, and API integration in crm-app/src."
name: "Leadfy Frontend Instructions"
applyTo: "crm-app/src/**"
---

# Leadfy Frontend Instructions

## Stack And Structure

- The frontend uses React, React Router, Context API, Axios, React Bootstrap, React Toastify and CSS Modules.
- Keep frontend code inside the existing structure under `crm-app/src`: `components`, `context`, `pages`, `routes`, `services` and `utils`.
- Match the current project pattern before introducing hooks, providers or shared abstractions that do not already exist nearby.

## Existing Naming Pattern

- Pages currently follow the project pattern `pages/Feature/Feature.js` with styles like `pages/Feature/_feature.module.css`.
- Reusable components follow `components/Component/Component.js` with styles like `components/Component/_component.module.css`.
- Service files use lowercase names ending in `Api.js`, such as `leadApi.js` and `opportunityApi.js`.
- Utility files usually use PascalCase names such as `FormatPhoneNumberUtil` or feature-scoped helpers under `utils/Feature`.

## Component And State Rules

- Keep components focused on a single screen concern or reusable UI concern.
- Prefer extracting repeated JSX or behavior into existing shared components when that pattern already exists.
- Keep API calls inside `services` and leave formatting and data normalization in `utils`.
- Reuse `AuthContext` and existing route guards for authentication-sensitive flows.

## Styling And UX

- Use CSS Modules following the current underscore-prefixed filenames already present in the repo.
- Preserve the current visual language and library usage instead of mixing in a new styling approach.
- Keep toast messages, modal flows and loading states consistent with nearby screens.

## Integration Rules

- Service modules should contain HTTP communication only and avoid UI logic.
- When sending update payloads, preserve fields that the backend expects even when the UI edits only part of the entity.
- Follow the existing route and page composition patterns in `crm-app/src/routes` and `crm-app/src/pages`.

## Validation

- Prefer `npm run build` in `crm-app` for compile validation.
- Use `npm test -- --watch=false` when the task affects tested behavior.
