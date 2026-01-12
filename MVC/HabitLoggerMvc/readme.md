# HabitLoggerMvc

A simple and intuitive ASP.NET Core MVC application to track your daily habits and stay consistent.

## 🚀 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Configuration

The application uses a SQLite database. By default, it will create a `habitlogger.db` file in the project root if no connection string is provided.

1. **Set your Connection String (Optional)**:
   You can override the default database location by adding a `DefaultConnection` to your `appsettings.json` or using .NET User Secrets:
   ```bash
   dotnet user-secrets set "ConnectionStrings:SqliteConnection" "Data Source=MyHabits.db"
   ```

2. **Database Initialization**:
   The application automatically initializes its schema on startup using Entity Framework Core.

## 🛠 Development

### Core Technologies
- **Frontend**: Razor Pages
- **Data Access**: Entity Framework Core
- **Database**: SQLite

### Project Structure
- `Repositories/`: Contains the data access logic for Habits and Logs.
- `Data/`: Contains the `HabitLoggerContext` responsible for database initialization.
- `Models/`: Domain objects representing habits and tracking units.

## 📝 Usage
- **Define Habits**: Create habits with specific units (e.g., "Water" in "Liters").
- **Log Activity**: Record your daily progress against your defined habits.
- **Track**: Use the interface to visualize your consistency and history.
```
