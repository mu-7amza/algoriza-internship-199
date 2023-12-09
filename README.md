# Vezeeta API

This is an ASP.NET Core Web API project built using Onion Architecture.

## Project Overview

The project is structured with separation of concerns in mind, following Onion Architecture principles. It includes layers for Core, Infrastructure, and Application.

### Project Structure

- **Core**: Contains entities, DTOs, and interfaces representing the core business logic.
- **Infrastructure**: Contains data access implementation, repositories, and utility classes.
- **Application**: Contains services and controllers responsible for handling business logic and API endpoints.
- **Web**: Entry point for the web application, includes Startup and configuration.

## Features

### 1. Repository Pattern

- **Repositories**: Interfaces and implementations for data access for each entity (e.g., `IUserRepository`, `UserRepository`).

### 2. Unit of Work

- **UnitOfWork**: Manages transactions and connection to the data source. Implements the `IUnitOfWork` interface.

### 3. Dependency Injection

- **Service Registration**: Dependencies such as repositories and the unit of work are registered in the dependency injection container in `Startup.cs`.

### 4. ASP.NET Core Web API

- **Controllers**: API endpoints are implemented in controllers within the `Application` layer.
- **DTOs**: Data Transfer Objects used for input and output to API.

### 5. Swagger Integration

- **Swagger**: API documentation and testing is available using Swagger. Access the Swagger UI at `/swagger` endpoint.

### 6. Sample Usage

- **User Service**: Example service (`UserService`) demonstrating how to use repositories and the unit of work in the application layer.

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)

### Installation

1. Clone the repository: `git clone https://github.com/your-username/vezeeta-api.git`
2. Navigate to the project directory: `cd vezeeta-api/Web`
3. Run the application: `dotnet run`

### API Documentation

Access the API documentation and test the endpoints using Swagger UI:

- [Swagger UI](http://localhost:5000/swagger)

## License

This project is licensed under the [MIT License](LICENSE).
