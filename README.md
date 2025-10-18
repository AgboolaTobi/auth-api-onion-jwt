Auth API - Onion Architecture with JWT
A secure authentication API built with .NET 8 using Clean/Onion Architecture principles and JWT token-based authentication.


🏗️ Architecture

This project follows the Onion Architecture pattern with clear separation of concerns:

Layers


Domain Layer: Contains core entities (User) and repository interfaces

Application Layer: Business logic, services, and Data Transfer Objects

Infrastructure Layer: Implements data access (SQLite), security (BCrypt), and JWT token generation

API Layer: ASP.NET Core Web API with controllers and Swagger documentation


🚀 Features

✅ User Registration with email validation
✅ User Login with JWT token generation
✅ Password hashing using BCrypt
✅ JWT token-based authentication
✅ Protected endpoints requiring authentication
✅ Swagger UI with JWT authorization support
✅ SQLite database for data persistence
✅ DataAnnotations for request validation

🛠️ Technologies

.NET 8 - Framework
Entity Framework Core - ORM
SQLite - Database
JWT Bearer Authentication - Security
BCrypt.Net - Password hashing
Swagger/OpenAPI - API documentation

📋 Prerequisites

.NET 8 SDK
Your favorite IDE (Visual Studio, VS Code, Rider)

⚙️ Configuration
Update appsettings.json in the Auth.Api project:
{
  "ConnectionStrings": {
    "Default": "Data Source=auth.db"
  },
  "Jwt": {
    "Secret": "your-super-secret-key-min-32-characters-long",
    "Issuer": "AuthApi",
    "Audience": "AuthApiUsers",
    "ExpiryMinutes": 60
  }
}


🚦 Getting Started


git clone https://github.com/AgboolaTobi/auth-api-onion-jwt.git

cd auth-api-onion-jwt


2. Restore Dependencies
3. 

dotnet restore

3. Apply Database Migrations

cd src/Auth.Api

dotnet ef database update

dotnet run

