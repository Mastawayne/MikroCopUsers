# 👤 MikroCopUsers API

A minimal ASP.NET Core Web API for managing users — supporting create, read, update, delete (CRUD), password validation, and localization.

---

## 📦 Tech Stack

- ASP.NET Core 8
- Entity Framework Core (In-Memory / SQL Server)
- xUnit (unit testing)
- Dependency Injection
- RESTful API principles
- Windows server 2022 (Optional for IIS and SQL SERVER)

---

## 🚀 Getting Started

### ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or any IDE that supports .NET
- SQL Server (local or remote) if not using In-Memory DB
---

## ⚙️ Configuration

1. **Clone the repository:**

git clone https://github.com/your-username/MikroCopUsers.git

Set up your appsettings.json:
Update the ConnectionStrings section:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=MikroCopUsersDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

If using InMemory DB (for tests), no config changes needed.

dotnet run --project MikroCopUsers
The API will be accessible at:

https://localhost:5079/api/users
https://localhost:5079/swagger
🧪 Running Tests
From the root directory:

dotnet test
📬 API Endpoints
Method	Endpoint	Description
GET	/api/users	Get all users
GET	/api/users/{id}	Get user by ID
POST	/api/users	Create a new user
PUT	/api/users/{id}	Update an existing user
DELETE	/api/users/{id}	Delete a user
POST	/api/users/validate-password	Validate login credentials

✅ Example JSON Payloads
🔐 Create User
{
  "userName": "mihael",
  "fullName": "Mihael Car",
  "email": "mihael@example.com",
  "mobileNumber": "123456789",
  "language": "Croatian",
  "culture": "Croatian",
  "passwordHash": "plaintext-password"
}
🔐 Login
{
  "userName": "mihael",
  "password": "plaintext-password"
}

📄 License
MIT — do what you want.

🧠 Credits
Developed by Damir Taraniš