# 🏨 Reservation API

A RESTful API built with ASP.NET Core for managing reservations, designed to support tourist-related services. This project includes JWT-based authentication and comprehensive unit testing with xUnit and Moq.

---

## 📦 Features

* **User Authentication**: Secure endpoints with JWT-based authentication.
* **Reservation Management**: Create, retrieve, and delete reservations.
* **Logging**: Custom logging implementation for tracking operations.
* **Testing**: Unit tests using xUnit and Moq for reliable codebase.

---

## 🚀 Getting Started

### Prerequisites

* [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* [SQLite](https://www.sqlite.org/download.html) (for local development)

### Installation

1. **Clone the repository**:

   ```bash
   git clone https://github.com/Cavacoz/reservation-api.git
   cd reservation-api
   ```

2. **Restore dependencies**:

   ```bash
   dotnet restore
   ```

3. **Apply migrations and update the database**:

   ```bash
   dotnet ef database update
   ```

4. **Run the application**:

   ```bash
   dotnet run
   ```

   The API will be available at `https://localhost:5001` or `http://localhost:5000`.

---

## 🔐 Authentication

This API uses JWT for authentication. To access protected endpoints:

1. **Register a new user** (if registration endpoint is available).
2. **Authenticate** using your credentials to receive a JWT token.
3. **Include the token** in the `Authorization` header for subsequent requests:

   ```
   Authorization: Bearer YOUR_JWT_TOKEN
   ```

---

## 🧪 Running Tests

Unit tests are located in the `ReservationAPITests` project.

To execute the tests:

```bash
dotnet test
```

Ensure that the test project has the necessary dependencies restored before running tests.

---

## 🛠️ Technologies Used

* **ASP.NET Core 9.0**: Web framework for building APIs.
* **Entity Framework Core**: ORM for database interactions.
* **SQLite**: Lightweight relational database for development.
* **JWT**: JSON Web Tokens for secure authentication.
* **xUnit**: Testing framework for .NET.
* **Moq**: Mocking library for unit tests.

---

## 📁 Project Structure

```
reservation-api/
├── ReservationAPI/           # Main API project
│   ├── Controllers/          # API Controllers
│   ├── Data/                 # Database context and migrations
│   ├── Models/               # Data models
│   ├── Services/             # Business logic services
│   └── Program.cs            # Application entry point
├── ReservationAPITests/      # Unit test project
│   └── ReservationControllerTests.cs
├── ReservationAPI.sln        # Solution file
└── README.md                 # Project documentation
```