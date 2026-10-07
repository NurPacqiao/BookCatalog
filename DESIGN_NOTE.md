# Design Note: Book Catalog Platform

---

## 1. System Architecture & Physical Boundaries

The service transitioned from a single-project folder structure to a strict, compiler-enforced layered architecture to eliminate architectural leaks:

```text
BookCatalog/
├── src/
│   ├── BookCatalog.Domain/          # Core entities, repository contracts, query specs (zero dependencies)
│   ├── BookCatalog.Infrastructure/  # In-memory persistence (ConcurrentDictionary)
│   └── BookCatalog.Api/             # Minimal APIs, DTOs, FluentValidation, middleware
└── tests/
    └── BookCatalog.UnitTests/       # Domain logic, validator boundaries, exception pipeline


- **Strict Dependency Inversion:** Dependencies flow in one direction only: `Api` → `Infrastructure` → `Domain`. The `Domain` layer has zero external references (no ASP.NET Core, EF, or JSON libraries). If persistence code tries to reference API contracts, the build halts.
- **Minimal APIs:** Selected over MVC controllers to reduce memory allocations, minimize cold starts, and align with modern cloud-native ASP.NET Core standards. Route handlers are modularized into extension classes (`BookEndpoints.cs`) rather than bloating `Program.cs`.


## 2. Concurrency, Storage & Querying

- **Thread-Safe In-Memory Store:** Persistence relies on `ConcurrentDictionary<Guid, Book>` registered as a Singleton, guaranteeing thread safety under concurrent requests.
- **Non-Blocking Contracts:** All repository methods return `Task`-based asynchronous signatures. This avoids thread pool starvation and ensures a drop-in replacement when migrating to Entity Framework Core without breaking method signatures.
- **Pagination & Slicing:** Query parameters (`search`, `year`, `page`, `pageSize`) are encapsulated in an immutable `BookQueryParameters` record. Filtering runs prior to counting metrics, followed by `Skip`/`Take` slicing. Results return via a generic `PagedResponse<T>` containing item arrays and total metadata.

## 3. Validation & Centralized Error Handling

- **FluentValidation:** Extracted business constraints (required fields, price > 0, realistic publication year bounds) into dedicated validator classes (`CreateBookRequestValidator`, `UpdateBookRequestValidator`).
- **RFC 7807/7231 ProblemDetails:** Replaced ad-hoc `try-catch` blocks with ASP.NET Core’s native `IExceptionHandler` (`GlobalExceptionHandler`).
- **Security & Consistency:** Unhandled runtime exceptions are intercepted, logged, and returned to clients as uniform 500 `ProblemDetails` payloads, preventing stack traces or internal server details from leaking.

## 4. Testing Strategy

Unit tests prioritize domain behavior, boundary conditions, and state mutations over testing framework wiring:

- **Service Logic (`BookServiceTests`):** Isolated via `NSubstitute` against `IBookRepository`. Verified both success paths (state mutations on update) and failure paths (not-found lookups returning `null`).
- **Validation Boundaries (`BookRequestValidatorTests`):** Used FluentValidation's `TestHelper` with parameterized tests (`[Theory]`, `[InlineData]`). Verified explicit edge conditions (e.g., blank strings, negative prices, publication year bounds like 867 failing vs. 868 passing).
- **Error Pipeline (`GlobalExceptionHandlerTests`):** Executed the handler directly against an in-memory `DefaultHttpContext` to assert HTTP status codes and JSON serialization in milliseconds without spinning up a test server.

## 5. What was hard & learned lessons

- **Contract Placement in Multi-Project Splits:** During layer extraction, `InMemoryBookRepository` broke because it referenced DTOs residing in the API layer. Moving query models down into `Domain` reinforced that data access contracts belong to the domain, not the transport layer.
- **Meaningful Mocking vs. Mocking Theater:** Focused tests on whether `UpdateAsync` actually passed transformed data to storage, rather than merely verifying that a method was invoked.
- **Preserving Git History:** Moving directories during project reorganization risks wiping file history into "delete + create" events. Executed moves using `git mv` in isolated commits to keep Git blame tracking intact.

## 6. Next steps I would do to improve

- **Database Migration:** Replace `InMemoryBookRepository` with Entity Framework Core and PostgreSQL / SQL Server.
- **Database-Level Queries:** Translate LINQ filtering and pagination directly into SQL `IQueryable` pushdown queries.
- **Containerization:** Add a multi-stage `Dockerfile` and `docker-compose.yml` to spin up the API and database locally with a single command.
```
