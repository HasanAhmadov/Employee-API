# 👥 Employee API 

A robust and modular **ASP.NET Core Web API** for managing employee data, attendance logs, permissions, and vacation requests. Built with clean architecture principles, **JWT authentication**, and **Entity Framework Core**.

## ✨ Features

- **🔐 JWT Authentication**: Secure login and endpoint protection.
- **🧱 Modular Architecture**: Clean separation into API, Business, Data Access, and Models layers.
- **📊 Employee Management**: Full CRUD operations for employee data.
- **⏱️ Attendance Logging**: Track employee check-ins and check-outs.
- **🖐️ Permission & Vacation Workflow**: Request and approve time-off with status tracking.
- **🔄 AutoMapper**: Simplified object-to-object mapping for DTOs.
- **🗄️ Entity Framework Core**: Code-first database approach with migrations.
- **📑 Swagger/OpenAPI**: Interactive API documentation.

## 🏗️ Project Architecture & Structure

This solution uses a layered architecture to promote separation of concerns, testability, and maintainability.
APIPractice.sln
├── APIPractice/ 
│ ├── Controllers/ 
│ ├── DTOs/ 
│ ├── Interfaces/ 
│ └── JWT/ 
│
├── MicroServices.BusinessLayer/ 
│ ├── Services/ 
│ ├── Interfaces/ 
│ └── Mappers/ 
│
├── MicroServices.DataAccessLayer/ 
│ ├── Data/ 
│ ├── Interfaces/ 
│ ├── Services/ 
│ └── Migrations/ 
│
└── MicroServices.Models/ 
├── Enums/ 
└── *.cs 

### Data Flow
`API Layer` → `Business Layer` → `Data Access Layer` → `Database`

The **Models** layer is shared across all other projects, representing the core domain entities.

## 📦 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (or another database provider supported by EF Core)

### Installation & Setup

1.  **Clone the Repository**
    ```bash
    git clone https://github.com/HasanAhmadov/Employee-API.git
    cd Employee-API
    ```

2.  **Configure the Database and JWT**
    - Navigate to the `APIPractice` project.
    - Open `appsettings.json` (or `appsettings.Development.json` for development).
    - Update the `ConnectionStrings.DefaultConnection` to point to your SQL Server instance.
    - Optionally, modify the `JwtSettings` (SecretKey, Issuer, Audience) for production use.

3.  **Apply Database Migrations**
    Open your terminal in the solution directory and run:
    ```bash
    dotnet ef database update --project MicroServices.DataAccessLayer --startup-project APIPractice
    ```
    *This command creates the database and applies all existing migrations.*

4.  **Run the Application**
    ```bash
    dotnet run --project APIPractice
    ```
    The API will start.

5.  **Explore the API with Swagger**
   
    Open your browser and navigate to Swagger UI. You will see all available endpoints and can test them interactively.

## 🔐 Authentication

1.  **First, obtain a JWT token:**
    ```http
    POST /api/Auth/login
    Content-Type: application/json

    {
      "username": "your_username",
      "password": "your_password"
    }
    ```

2.  **Use the token to access protected endpoints:**
    In Swagger, click the "Authorize" button (lock icon) and enter your token as `Bearer <your_token>`.
    Or, via HTTP:
    ```http
    GET /api/Employee
    Authorization: Bearer <your_jwt_token_here>
    ```

## 🧪 API Endpoints Overview

| Resource | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| **Auth** | `POST /api/Auth/login` | Authenticates a user and returns a JWT token. | No |
| **Employees** | `GET /api/Employee` | Retrieves a list of all employees. | Yes |
| **Employee Logs** | `POST /api/EmployeeLog` | Creates a new check-in/check-out log. | Yes |
| **Permissions** | `POST /api/Permission` | Submits a new permission request. | Yes |
| **Vacations** | `POST /api/Vacation` | Submits a new vacation request. | Yes |

*For a complete list of all endpoints, please explore the Swagger UI.*

## 🙏 Acknowledgments

- [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) team for the excellent framework.
- The community for providing invaluable packages and tutorials.
