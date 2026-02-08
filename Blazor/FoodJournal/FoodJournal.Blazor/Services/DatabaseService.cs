using System.Data;
using System.Reflection;
using Dapper;
using FoodJournal.Blazor.Models.Data;
using Microsoft.Data.Sqlite;
using static FoodJournal.Blazor.Utils.IngredientNameNormalizer;

namespace FoodJournal.Blazor.Services;

public interface IDatabaseService
{
    IDbConnection GetConnection();
    Task CreateDatabase();
}

public class DatabaseService(IConfiguration configuration) : IDatabaseService
{
    public IDbConnection GetConnection()
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        return new SqliteConnection(connectionString);
    }

    public async Task CreateDatabase()
    {
        await CreateTables();
        await SeedDatabase();
    }

    private async Task SeedDatabase()
    {
        using var connection = GetConnection();
        var isSeeded =
            await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM SeedHistory WHERE TableName = 'Meals'");
        if (isSeeded > 0) return;

        var seedMeals = new List<Meal>
        {
            new()
            {
                Name = "Flan",
                Description =
                    "For the caramel place 100 grams of sugar in a frying pan without any fat and melt on medium heat. Try not to stir with a spoon, but swirl the pan itself. Let melt until you have an amber color and sugar is fully melted. Fill the bottom of four flan containers with it.",
                ThumbnailUrl = "https://www.themealdb.com/images/media/meals/0s80wo1764374393.jpg",
                Type = MealType.Snack,
                Date = DateTime.Parse("2026-02-07 09:32:00"),
                Ingredients =
                [
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Sugar" }, Measurement = "100g " },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Milk" }, Measurement = "350g" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Sugar" }, Measurement = "45g" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "vanilla pod" }, Measurement = "1" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Egg Yolks" }, Measurement = "4" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Egg" }, Measurement = "2" },
                    new RecipeIngredient
                        { Ingredient = new Ingredient { Name = "Dulce de leche" }, Measurement = "To taste" }
                ]
            },
            new()
            {
                Name = "Ezme",
                Description =
                    "Put the tomatoes and all of the peppers in a food processor and blitz until finely chopped. Tip out into a sieve, set over a bowl and leave to strain. Add the onions, garlic and parsley to the food processor and blitz until finely chopped, then set aside.",
                ThumbnailUrl = "https://www.themealdb.com/images/media/meals/pb6mj11763788331.jpg",
                Type = MealType.Snack,
                Date = DateTime.Parse("2026-02-07 09:32:00"),
                Ingredients =
                [
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Tomato" }, Measurement = "3 Large" },
                    new RecipeIngredient
                        { Ingredient = new Ingredient { Name = "Romano Pepper" }, Measurement = "1 medium" },
                    new RecipeIngredient
                        { Ingredient = new Ingredient { Name = "Green Chilli" }, Measurement = "1 medium" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Onion" }, Measurement = "1 small" },
                    new RecipeIngredient
                        { Ingredient = new Ingredient { Name = "Garlic" }, Measurement = "2 cloves minced" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Parsley" }, Measurement = "25g" }
                ]
            },
            new()
            {
                Name = "kabse",
                Description =
                    "Caramelize the chicken with olive oil then add a maggi cube and boil it for around 30 minutes. In another pot, add all the veggies and caramelize them till they are soft.",
                ThumbnailUrl = "https://www.themealdb.com/images/media/meals/utqnjv1763598650.jpg",
                Type = MealType.Dinner,
                Date = DateTime.Parse("2026-02-07 09:32:00"),
                Ingredients =
                [
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Chicken" }, Measurement = "400g" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Rice" }, Measurement = "1 cup" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Green Pepper" }, Measurement = "1" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Red Pepper" }, Measurement = "1" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Mushrooms" }, Measurement = "2" }
                ]
            },
            new()
            {
                Name = "Asado",
                Description =
                    "Prepare the Fire: Start a wood fire in your grill and let it burn down to coals. Season the Meat: Generously salt the beef cuts. Grill the Meat: Place the beef on the grill, starting with the thickest cuts farthest from the coals.",
                ThumbnailUrl = "https://www.themealdb.com/images/media/meals/kgfh3q1763075438.jpg",
                Type = MealType.Dinner,
                Date = DateTime.Parse("2026-02-07 09:32:00"),
                Ingredients =
                [
                    new RecipeIngredient
                        { Ingredient = new Ingredient { Name = "Mixed Beef Cuts" }, Measurement = "2kg" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Chorizo" }, Measurement = "4" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Morcilla" }, Measurement = "2" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Salt" }, Measurement = "To taste" }
                ]
            },
            new()
            {
                Name = "Migas",
                Description =
                    "Crumble the bread into small pieces. Sprinkle with cold water, cover with a damp cloth and leave for 30 minutes. Heat 2 tsp of olive oil in a deep pan.",
                ThumbnailUrl = "https://www.themealdb.com/images/media/meals/xd9aj21740432378.jpg",
                Type = MealType.Breakfast,
                Date = DateTime.Parse("2026-02-07 09:32:00"),
                Ingredients =
                [
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Bread" }, Measurement = "Crumbs" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Olive Oil" }, Measurement = "2 tsp" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Garlic" }, Measurement = "3 cloves" },
                    new RecipeIngredient { Ingredient = new Ingredient { Name = "Pork" }, Measurement = "100g" }
                ]
            }
        };

        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            foreach (var meal in seedMeals)
            {
                const string insertMealSql = """
                                             INSERT INTO Meals (Name, Description, ThumbnailUrl, Type, Date)
                                             VALUES (@Name, @Description, @ThumbnailUrl, @Type, @Date);
                                             SELECT last_insert_rowid();
                                             """;
                var mealId = await connection.ExecuteScalarAsync<int>(insertMealSql, meal, transaction);

                foreach (var ingredient in meal.Ingredients)
                {
                    ingredient.Ingredient.Name = NormaliseKey(ingredient.Ingredient.Name);
                    const string insertIngredientSql = """
                                                       INSERT INTO MealIngredients (MealId, IngredientName, Measurement)
                                                       VALUES (@MealId, @IngredientName, @Measurement);
                                                       """;
                    await connection.ExecuteAsync(insertIngredientSql, new
                    {
                        MealId = mealId,
                        IngredientName = ingredient.Ingredient.Name,
                        ingredient.Measurement
                    }, transaction);
                }
            }

            await connection.ExecuteAsync("INSERT INTO SeedHistory (TableName) VALUES ('Meals')", transaction);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private async Task CreateTables()
    {
        using var connection = GetConnection();

        var seedHistorySql = await GetSqlFromResource("FoodJournal.Blazor.Scripts.000_CreateSeedHistory.sql");
        await connection.ExecuteAsync(seedHistorySql);

        var createMealsSql = await GetSqlFromResource("FoodJournal.Blazor.Scripts.001_CreateMealsTable.sql");
        await connection.ExecuteAsync(createMealsSql);

        var createMealIngredientsSql = await GetSqlFromResource(
            "FoodJournal.Blazor.Scripts.002_CreateMealIngredientsTable.sql");
        await connection.ExecuteAsync(createMealIngredientsSql);
    }

    private async Task<string> GetSqlFromResource(string name)
    {
        await using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        if (stream == null) throw new InvalidOperationException($"Resource '{name}' not found");

        using var reader = new StreamReader(stream);

        return await reader.ReadToEndAsync();
    }
}