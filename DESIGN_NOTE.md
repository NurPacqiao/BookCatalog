# Design Note – Week 1: Book Catalog API

## 1. Architectural Decisions

- **Minimal APIs:** Chosen over MVC controllers for reduced memory allocation overhead, faster startup times, and alignment with modern cloud-native ASP.NET Core patterns.
- **Modular Endpoints:** To prevent a bloated `Program.cs`, route handlers are organized into dedicated extension classes (`BookEndpoints.cs`).
- **Layered Separation of Concerns:**
  - **Transport Layer (`Endpoints/`):** Focuses solely on HTTP binding and status code mapping.
  - **Business Layer (`Services/`):** Manages domain logic, service-level logging, and DTO coordination.
  - **Persistence Layer (`Repositories/`):** Encapsulates storage mechanics behind an asynchronous `IBookRepository` contract to allow zero-friction migration to Entity Framework Core.

## 2. Data Validation & Error Handling

- **FluentValidation:** Business constraints (required fields, price > 0, valid publication year bounds) are isolated into reusable validator classes (`CreateBookRequestValidator`, `UpdateBookRequestValidator`).
- **RFC 7807 Standard:** Validation errors return standardized problem details payloads via `Results.ValidationProblem()`.

## 3. Concurrency & Asynchronous Design

- In-memory storage utilizes `ConcurrentDictionary<Guid, Book>` to ensure thread safety across concurrent HTTP requests.
- All repository and service methods use `Task`-based asynchronous signatures to prevent thread pool starvation and maintain compatibility with future database drivers.
