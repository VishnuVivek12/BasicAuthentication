# ASP.NET Web API with Basic Authentication and Role-Based Authorization

This project demonstrates how to implement **Basic Authentication** and **Role-Based Authorization** in an **ASP.NET Web API** application. It includes Swagger integration for API testing and uses XML documentation for API endpoints.

---

## 📝 Features

- 🔐 **Basic Authentication** using a custom authorization filter.
- 👥 **Role-based access control** for API endpoints.
- 🧪 Integrated **Swagger UI** for testing APIs.
- 💬 **XML comments** for API documentation.
- 🎯 Demonstrates the use of `HttpContext` and `Thread.CurrentPrincipal` for identity access.

---

## 🧰 Technologies and Skills Used

- **ASP.NET Web API**
- **C#**
- **Swagger (Swashbuckle)**
- **Basic Authentication Filter**
- **Custom Authorization Filters**
- **Threading and HttpContext for Identity**
- **Role-based security using [Authorize] attributes**
- **XML Documentation Comments**

---

## 📁 Project Structure

```
APIBasicAuth/
│
├── Controllers/
│ └── EmployeeController.cs # Main controller handling employee operations
│
├── Filters/
│ └── BasicAuthenticationAttribute.cs # Custom basic authentication logic
│
├── Models/
│ ├── Employee.cs # Employee model
│ └── User.cs # User model with username, password, roles
│
└── App_Start/
└── SwaggerConfig.cs # Swagger configuration
```

---

## 🚀 Getting Started

### Prerequisites

- Visual Studio
- .NET Framework 4.7+ or compatible
- NuGet packages: `Swashbuckle`

### Run Instructions

1. Clone the repository.
2. Open the solution in Visual Studio.
3. Build and run the project.
4. Navigate to: http://localhost:port/swagger
5. Use Basic Auth header to authorize and test endpoints.
