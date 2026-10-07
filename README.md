# Book Catalog Platform API

A layered REST API built with ASP.NET Core (.NET 10) for managing a book catalog, featuring in-memory persistence, request validation, centralized error handling, and unit test coverage.

---

## Architecture & Project Structure

The solution enforces strict compiler-level separation of concerns across standalone class libraries:

````text
BookCatalog/
├── src/
│   ├── BookCatalog.Domain/          # Core business entities, contracts, and query models (zero dependencies)
│   ├── BookCatalog.Infrastructure/  # Data access implementation (InMemoryBookRepository)
│   └── BookCatalog.Api/             # Minimal APIs, DTOs, FluentValidation, and middleware
├── tests/
│   └── BookCatalog.UnitTests/       # Domain logic, validator boundaries, and pipeline tests
├── BookCatalog.slnx                 # Solution configuration
└── DESIGN_NOTE.md                   # Architectural decisions, trade-offs, and design evolution

- **Domain:** Pure business models (`Book`), query specifications (`BookQueryParameters`), and repository abstractions (`IBookRepository`).
- **Infrastructure:** Implements storage mechanics using thread-safe `ConcurrentDictionary`. Depends only on `Domain`.
- **Api:** Presentation layer handling HTTP routing, model binding, and dependency injection orchestration.
- **UnitTests:** Isolated test suite verifying business logic, validation boundaries, and exception handling.

## Tech Stack

- **Runtime & Language:** .NET 10 / C# 14
- **Framework:** ASP.NET Core Minimal APIs
- **API Documentation:** Swagger / OpenAPI & Scalar
- **Validation:** FluentValidation
- **Error Handling:** RFC 7807 ProblemDetails via `IExceptionHandler`
- **Testing:** xUnit, FluentAssertions, NSubstitute

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run the Application
```bash
dotnet run --project src/BookCatalog.Api

### API Documentation & Interactive UI

Once running, explore and test the endpoints using either interface:
- **Scalar UI:** `https://localhost:5191/scalar/v1`
- **Swagger UI:** `https://localhost:5191/swagger`

*(Port may vary depending on your local launch profile).*

### Run Unit Tests

```bash
dotnet test


## API Endpoints

| Method | Endpoint | Description | Parameters / Payload | Status Codes |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/books` | Retrieve paginated books | Query: `search`, `year`, `page`, `pageSize` | `200 OK` |
| `GET` | `/books/{id}` | Retrieve a book by ID | Route: `id` (GUID) | `200 OK`, `404 Not Found` |
| `POST` | `/books` | Create a new book | Body: `CreateBookRequest` (JSON) | `201 Created`, `400 Bad Request` |
| `PUT` | `/books/{id}` | Update existing book details | Route: `id` (GUID)<br>Body: `UpdateBookRequest` (JSON) | `204 NoContent`, `400 Bad Request`, `404 Not Found` |
| `DELETE` | `/books/{id}` | Remove a book from the catalog | Route: `id` (GUID) | `204 NoContent`, `404 Not Found` |
````
