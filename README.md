# .NET 8 CQRS & Event-Driven Order API

This project is a .NET 8 ASP.NET Core Web API demonstrating the **Command Query Responsibility Segregation (CQRS)** pattern with an event-driven read model. It's designed to manage customer orders, separating the process of creating and modifying data from the process of querying it.

## 1. Architecture & Flow

The core principle of this application is the clear separation of concerns between the "write" side (commands) and the "read" side (queries).

- **Write Side (Commands):**
    1. A `CreateOrderCommand` is received by the API.
    2. The command is first validated using `FluentValidation`.
    3. The `CreateOrderCommandHandler` processes the command, saving the new `Order` entity to the **write database** via `WriteDbContext`.
    4. Upon successful creation, an `OrderCreatedEvent` is published.

- **Projection (Event Handling):**
    1. The `OrderCreatedProjectionHandler` listens for the `OrderCreatedEvent`.
    2. It updates the **read database** by creating a simplified representation of the order, keeping the read model in sync with the write model.

- **Read Side (Queries):**
    1. API endpoints for listing or getting a specific order receive queries like `GetOrdersListQuery` or `GetOrderByIdQuery`.
    2. These queries directly access the **read database** via `ReadDbContext`.
    3. They return lightweight Data Transfer Objects (DTOs), specifically designed for API consumption, without exposing the full `Order` entity.

## 2. Key Components

- **`Features/Commands/*`**: Contains all command handlers (`CreateOrderCommandHandler.cs`), their associated commands (`CreateOrderCommand.cs`), and validation logic (`CreateOrderCommandValidator.cs`).
- **`Features/Queries/*`**: Holds the query handlers (`GetOrderByIdQueryHandler.cs`) and query objects (`GetOrdersListQuery.cs`).
- **`Services/Events/*`**: Defines the event publishing mechanism. The `IEventPublisher` interface is implemented by `InProcessEventPublisher`, which handles events within the same application process. A `ConsoleEventPublisher` is also included for demonstration and logging.
- **`Projections/*`**: The projection logic lives here. `OrderCreatedProjectionHandler.cs` is the key component responsible for updating the read model based on new events.
- **`Data/`**: Manages persistence.
    - **`WriteDbContext.cs`**: The EF Core `DbContext` for the write database (commands).
    - **`ReadDbContext.cs`**: The EF Core `DbContext` for the read database (queries).
    - **`DB/`**: The folder containing the actual SQLite database files (`write.db` and `read.db`).
- **`Dtos/*`**: A collection of Data Transfer Objects (`OrderDto.cs`) used for API responses and preventing the exposure of internal data models.
- **`Migrations/`**: Migrations are separated by database to maintain schema control for both the write and read models.
    - **`Migrations/Read`**: Migrations for `ReadDbContext`.
    - **`Migrations/Write`**: Migrations for `WriteDbContext`.
- **`Program.cs`**: The application's entry point, likely using minimal APIs to configure the request pipeline, dependency injection, database contexts, Swagger, and endpoint mapping.

## 3. Technologies Used

- **.NET 8 / ASP.NET Core**: The core framework for building the API.
- **Entity Framework Core**: An object-relational mapper (ORM) used for database interactions.
- **SQLite**: A lightweight, file-based database used for both the read and write models, simplifying the setup without requiring an external database server.
- **FluentValidation**: A popular .NET library for building strong-typed validation rules.
- **Dependency Injection**: The built-in DI container of ASP.NET Core is used to manage service lifetimes and resolve dependencies.
- **OpenAPI / Swagger**: Provides an interactive API documentation page and metadata, making it easy to test and understand the endpoints.

## 4. Getting Started

1.  **Clone the repository:**
    ```bash
    git clone [your-repo-url]
    cd [your-project-directory]
    ```

2.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```

3.  **Run migrations:**
    The databases are managed by separate migrations. Apply them to ensure your database schemas are up to date.
    ```bash
    # Apply Write DB migrations
    dotnet ef database update --context WriteDbContext --project Data --startup-project [YourProjectName]

    # Apply Read DB migrations
    dotnet ef database update --context ReadDbContext --project Data --startup-project [YourProjectName]
    ```
    *Note: Replace `[YourProjectName]` with the name of your main project file, e.g., `MyCqrsApi.csproj`.*

4.  **Run the application:**
    ```bash
    dotnet run
    ```
    The API will be available at `https://localhost:5001` (or a similar port). The Swagger UI can be accessed at `https://localhost:5001/swagger`.

## 5. API Endpoints

- **`POST /api/orders`**: Creates a new order.
- **`GET /api/orders`**: Retrieves a list of all orders from the read model.
- **`GET /api/orders/{id}`**: Retrieves a specific order by ID from the read model.
