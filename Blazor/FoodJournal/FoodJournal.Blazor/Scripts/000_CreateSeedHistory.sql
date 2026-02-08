CREATE TABLE
    IF NOT EXISTS SeedHistory
(
    TableName TEXT,
    DateAdded DATETIME default CURRENT_TIMESTAMP
);