# BudgetApp

A simple budget management application built with ASP.NET Core MVC and Entity Framework Core.

## Features

- Track transactions (Income/Expenses)
- Categorize transactions
- View transaction history

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or Express)

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd BudgetApp
```

### 2. Configure the Database

The application uses SQL Server. You need to provide a connection string named `DefaultConnection`.

You can add it to `BudgetApp/appsettings.json` or use .NET User Secrets:

#### Using appsettings.json

Edit `BudgetApp/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BudgetDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  ...
}
```

#### Using User Secrets

Run the following command in the `BudgetApp` project directory:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=BudgetDB;Trusted_Connection=True;MultipleActiveResultSets=true"
```

### 3. Run the Application

Navigate to the project directory and run the application:

```bash
cd BudgetApp
dotnet run
```

The application will automatically create the database on the first run using `EnsureCreated()`.

### 4. Access the App

Open your browser and navigate to:
`https://localhost:5001` (or the port specified in the console output)
