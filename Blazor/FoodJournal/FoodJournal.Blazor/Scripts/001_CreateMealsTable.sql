CREATE TABLE
    IF NOT EXISTS Meals
(
    Id           INTEGER PRIMARY KEY AUTOINCREMENT,
    Name         NVARCHAR(200) NOT NULL,
    Description  TEXT,
    ThumbnailUrl NVARCHAR(200) NOT NULL,
    Type         INT,
    Date         DATETIME DEFAULT CURRENT_TIMESTAMP
);