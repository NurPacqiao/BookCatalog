# Book Catalog Platform API

A layered REST API built with ASP.NET Core (.NET 10) for managing a catalog of books, featuring in-memory persistence, request validation, centralized error handling, and unit test suites.

---

## Architecture & Project Structure

The solution follows a multi-project **Layered Architecture** to enforce separation of concerns:

````text
BookCatalog/
├── src/
│   ├── BookCatalog.Domain/          # Core business entities, interfaces, and shared records
│   ├── BookCatalog.Infrastructure/  # Persistence implementations (In-Memory Repository)
│   └── BookCatalog.Api/             # Minimal API endpoints, DTOs, mappings, middleware
├── tests/
│   └── BookCatalog.UnitTests/       # Unit tests for domain logic, validators, and error handling
├── BookCatalog.slnx                 # Solution configuration
└── DESIGN_NOTE.md                   # Architectural decisions and technical notes



Architectural Boundaries
- Domain: Zero external dependencies. Declares the Book entity, IBookRepository contract, and query parameter types.
- Infrastructure: References Domain. Contains data access logic (InMemoryBookRepository).
- Api: References Domain and Infrastructure. Exposes HTTP endpoints, handles serialization, and injects dependencies.
- UnitTests: Isolated test suite verifying business logic, request validation rules, and middleware.

Tech Stack & Tools
- Runtime & Framework: .NET 10 / C# 14
- Web Framework: ASP.NET Core Minimal APIs
- Validation: FluentValidation
- Testing: xUnit, FluentAssertions, NSubstitute
- Error Handling: RFC 7231 ProblemDetails (IExceptionHandler)

Getting Started
Prerequisites
- .NET 10 SDK

Run the Application
From the repository root:
dotnet run --project src/BookCatalog.Api

The API will start and display local localhost URLs (e.g., https://localhost:5191).

Run Unit Tests
Execute the full test suite across the solution:
dotnet test

API Endpoints
Method	Endpoint	Description
GET	    /books	    Retrieve paginated books (supports search, year, page, pageSize)
GET	    /books/{id}	Retrieve a book by its UUID
POST	/books	    Create a new book with payload validation
PUT	    /books/{id}	Update existing book details
DELETE	/books/{id}	Remove a book by UUID

### 3. Stage, Commit, and Push

Run:

```bash
# 1. Check status
git status

# 2. Stage changes
git add DESIGN_NOTE.md README.md

# 3. Commit
git commit -m "docs: add root README and relocate design note"

# 4. Push branch to GitHub
git push origin <your-branch-name>
````
