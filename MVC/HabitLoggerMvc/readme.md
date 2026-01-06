# HabitLoggerMvc

A simple and intuitive ASP.NET Core MVC application to track your daily habits and stay consistent.

## 🚀 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Configuration

The application uses a SQL Server database. To get started, you need to provide a connection string. 

1. **Set your Connection String**:
   You can add it to your `appsettings.json` or use .NET User Secrets (recommended for development):
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER;Database=HabitLogger;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True"
   ```

2. **Database Initialization**:
   The application is designed to automatically initialize its schema on startup. Ensure the user in your connection string has permissions to create tables.

## 🛠 Development

### Core Technologies
- **Frontend**: Razor Pages
- **Data Access**: Dapper (Lightweight ORM)
- **Database**: SQL Server / Microsoft.Data.SqlClient

### Project Structure
- `Repositories/`: Contains the data access logic for Habits and Logs.
- `Data/`: Contains the `HabitLoggerContext` responsible for database initialization.
- `Models/`: Domain objects representing habits and tracking units.

## 📝 Usage
- **Define Habits**: Create habits with specific units (e.g., "Water" in "Liters").
- **Log Activity**: Record your daily progress against your defined habits.
- **Track**: Use the interface to visualize your consistency and history.
