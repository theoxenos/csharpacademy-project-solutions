# Movie Collection App

A modern ASP.NET Core MVC application to manage your favorite movies.

## Features
- **Full CRUD**: Create, Read, Update, and Delete movies.
- **Search**: Filter movies by title.
- **Filter**: Filter movies by genre.
- **Sorting**: Sort movies by Title, Release Date, Genre, Price, and Rating.
- **Responsive UI**: Modern dark-themed interface built with Bootstrap and Bootstrap Icons.

## Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server (or LocalDB)

## Getting Started

1.  **Clone the repository**
    ```bash
    git clone <repository-url>
    cd MvcMovie
    ```

2.  **Update the Database**
    The application uses Entity Framework Core. Run the following command to apply migrations and seed the initial data:
    ```bash
    dotnet ef database update --project MvcMovie
    ```
    *Note: Ensure your connection string in `MvcMovie/appsettings.json` is correctly configured for your environment.*

3.  **Run the application**
    ```bash
    dotnet run --project MvcMovie
    ```

4.  **Access the app**
    Open your browser and navigate to:
    - [https://localhost:7158](https://localhost:7158)
    - [http://localhost:5245](http://localhost:5245)

## Technology Stack
- **Backend**: ASP.NET Core 10 MVC
- **Database**: SQL Server with Entity Framework Core
- **Frontend**: Razor Views, Bootstrap 5, Bootstrap Icons
