CREATE TABLE IF NOT EXISTS Ingredients
(
    Id            INTEGER PRIMARY KEY AUTOINCREMENT,
    Name          TEXT NOT NULL,
    Description   TEXT,
    ThumbnailUrl  TEXT,
    Type          TEXT,
    Calories      REAL,
    Carbohydrates REAL,
    Fat           REAL,
    Protein       REAL
)