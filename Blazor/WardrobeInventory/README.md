# Wardrobe Inventory

A Blazor Server application to manage your personal wardrobe inventory. Built with .NET 10, MudBlazor, and SQLite.

## Features

- **Manage Wardrobe Items**: Add, view, edit, and delete wardrobe items.
- **Image Upload**: Upload images for each wardrobe item.
- **Categorization**: Organize items by category (e.g., Tops, Bottoms, Shoes) and size.
- **Responsive Design**: Clean and modern UI using MudBlazor components.
- **Data Persistence**: Uses SQLite for lightweight, local data storage.

## Technologies Used

- **Framework**: Blazor Server (.NET 10)
- **UI Library**: MudBlazor
- **Database**: SQLite
- **ORM**: Entity Framework Core

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Getting Started

1. **Clone the repository**:
   ```bash
   git clone <repository_url>
   cd WardrobeInventory
   ```

2. **Database Setup**:
   The application uses SQLite. The database file will be created automatically upon first run. Migrations are already included in the project.

3. **Run the application**:
   ```bash
   dotnet run --project WardrobeInventory.Blazor
   ```

4. **Access the app**:
   Open your browser and navigate to the URL shown in the terminal.

## Usage

- **Home Page**: Displays all items in your wardrobe as cards.
- **Add Item**: Click the "New Item" button to open a dialog for adding a new piece of clothing.
- **Edit/Delete**: Each item card has options to edit details or remove the item from your inventory.

## Testing

This project includes comprehensive unit tests to ensure code quality and reliability.

### Test Framework

- **Testing Framework**: xUnit
- **Database Testing**: Entity Framework Core InMemory Provider

### Test Structure

The test project (`WardrobeInventory.Tests`) is organized as follows:
```

WardrobeInventory.Tests/
├── UnitTests/
│   ├── WardrobeItemTests.cs      # Model validation tests
│   ├── WardrobeServiceTests.cs   # Service layer tests
│   └── ImageServiceTests.cs      # Image handling tests
└── Utils/
    └── TestDbContextFactory.cs   # Test helper for database context
```
### Test Coverage

#### Model Validation Tests (`WardrobeItemTests`)
- Valid model creation and validation
- String length validation (edge cases: minimum, maximum, and invalid lengths)
- Enum validation for all Category values (Shirts, Pants, Dress, Shoes)
- Enum validation for all Size values (S, M, L, XL)

#### Service Tests (`WardrobeServiceTests`)
- **Add Operations**: Adding items to database, null handling
- **Read Operations**: Retrieving all items, retrieving by ID, handling non-existent items
- **Update Operations**: Updating existing items, null handling
- **Delete Operations**: Deleting items, handling non-existent items, error scenarios

#### Image Service Tests (`ImageServiceTests`)
- Image upload and storage functionality
- Image retrieval and validation

### Running Tests

Run all tests:
```bash
dotnet test
```
Run tests with detailed output:
```bash
dotnet test --verbosity normal
```
Run tests with code coverage:
```bash
dotnet test --collect:"XPlat Code Coverage"
```
Run specific test file:
```bash
dotnet test --filter "FullyQualifiedName~WardrobeServiceTests"
```

## About

### Lessons
I used an in‑memory database for the unit tests. I’m not sure if that’s ideal, 
since it makes the tests feel more like integration tests. However, the examples of mocking DbContext were confusing 
and seemed excessive.

Firt time using Mudblazor. It was great to see the components and how easy it was to use.