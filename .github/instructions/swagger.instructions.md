---
description: "Use when creating or updating ASP.NET Core API endpoints, controller actions, Swagger configuration, XML comments, ProducesResponseType attributes, request/response documentation, and OpenAPI metadata in the backend."
name: "Leadfy Swagger And API Documentation Instructions"
applyTo: "backend/Api/Controllers/**/*.cs, backend/Api/Program.cs"
---

# Leadfy Swagger And API Documentation Instructions

## When This Applies

- Use these rules for new endpoints, changes to controller actions, response contracts, or Swagger/OpenAPI configuration.
- Apply the same care when changing `backend/Api/Program.cs`, because Swagger setup is centralized there.

## Endpoint Documentation

- Every public endpoint should have XML documentation with, at minimum, a summary, parameter descriptions when relevant, returns context, and expected HTTP response codes.
- Keep the text objective and API-focused: describe what the endpoint does, what it receives, and what it returns.
- Write summaries in the same language already used in the controller being edited. In this repository, Portuguese is the default for controller documentation.
- Document business-relevant route parameters, query parameters and body objects with `<param>` tags when they are part of the action signature.

## Response Metadata

- Add `ProducesResponseType` attributes that reflect the actual behavior of the action.
- Include the common success and failure codes handled by the controller, such as `200`, `201`, `204`, `400`, `401`, `404`, and `500` when they are truly possible.
- Keep the documented status codes aligned with the exception handling and return statements in the method.
- Prefer `ActionResult` or `ActionResult<T>` signatures when that improves response clarity in Swagger.

## Consistency With Existing Controllers

- Match the current controller style already used in `backend/Api/Controllers`.
- Keep controllers thin: documentation should describe the HTTP contract, not duplicate business rules that belong to the application layer.
- When using `CreatedAtAction`, `NoContent`, `Ok`, `BadRequest`, or `NotFound`, make sure the XML docs and `ProducesResponseType` list stay synchronized with those results.

## Swagger Configuration

- When editing Swagger setup in `backend/Api/Program.cs`, preserve XML comment inclusion and JWT Bearer security documentation unless the task explicitly changes authentication behavior.
- If a new API behavior affects authentication or global metadata, update Swagger configuration in the smallest possible way.

## Practical Rule

- If an endpoint change modifies route shape, parameters, status codes, authentication requirements, or returned payloads, update the XML comments and Swagger metadata in the same change.
