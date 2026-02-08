CREATE TABLE
    IF NOT EXISTS MealIngredients
(
    Id             INTEGER PRIMARY KEY AUTOINCREMENT,
    MealId         INT           not null
        constraint MealIngredients_Meals_Id_fk references Meals,
    IngredientName NVARCHAR(200) not null,
    Measurement    NVARCHAR(200) not null
);

CREATE INDEX IF NOT EXISTS IX_MealIngredients_IngredientName
    ON MealIngredients (IngredientName);