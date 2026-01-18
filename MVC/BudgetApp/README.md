# BudgetApp

A simple budget management application built with ASP.NET Core MVC and Entity Framework Core.

## 🚀 Features

- **Transaction Management**: Add, update, and delete income/expenses.
- **Categorization**: Group transactions by categories with visual color coding.
- **Dynamic Filtering**: Filter transactions by name, date, and category without page reloads.
- **AJAX-powered UI**: Smooth user experience using AJAX for CRUD operations and filtering.

## 🛠️ Tech Stack

- **Backend**: ASP.NET Core 10 (MVC)
- **Database**: SQL Server with Entity Framework Core
- **Frontend**: Razor Views, Bootstrap 5, jQuery, AJAX
- **Tools**: .NET User Secrets for secure configuration

## 📁 Project Structure

```text
BudgetApp/
├── Controllers/         # MVC Controllers (Transactions, Categories, Home)
├── Data/                # EF Core DbContext and database initialization logic
├── Models/              # Domain entities and ViewModels
├── Validations/         # Custom validation attributes (e.g., IdValidation)
├── Views/               # Razor views organized by controller
└── wwwroot/             # Static assets (CSS, JS, Libraries)
    ├── js/              # Custom JavaScript for AJAX operations
    └── css/             # Custom styling
```

## 🏗️ Getting Started

### 1. Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or Express)

### 2. Clone the repository

```bash
git clone <repository-url>
cd BudgetApp
```

### 3. Configure the Database

The application uses SQL Server. You need to provide a connection string named `DefaultConnection`.

#### Using User Secrets (Recommended)

Run the following command in the `BudgetApp` project directory:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=BudgetDB;Trusted_Connection=True;MultipleActiveResultSets=true"
```

#### Using appsettings.json

Alternatively, edit `BudgetApp/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BudgetDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 4. Run the Application

Navigate to the project directory and run:

```bash
cd BudgetApp
dotnet run
```

> **Note**: The application automatically creates and seeds the database on the first run using `context.Database.EnsureCreated()`.

### 5. Access the App

Open your browser and navigate to:
`https://localhost:5001` (or the port specified in the console output)

## 🔍 Key Implementation Details for Review

- **Database Initialization**: Check `Program.cs` for the `EnsureCreated()` logic which simplifies the initial setup by creating and seeding the database automatically.
- **Seed Data**: Pre-defined categories and sample transactions are configured in `BudgetContext.OnModelCreating`.
- **AJAX Integration**: CRUD operations and filtering in `TransactionsController` return `PartialView` results, which are handled by `wwwroot/js/transactions.js` to update the DOM without full page reloads.
- **Custom Validation**: See `Validations/IdValidation.cs` for a custom attribute used to ensure valid category selection.
- **Visual Feedback**: Categories use color coding defined in the database, reflected in the UI via inline styles.

## 🤝 Credits

- **JetBrains Junie**: For assistance with the UI implementation and code refactoring.
