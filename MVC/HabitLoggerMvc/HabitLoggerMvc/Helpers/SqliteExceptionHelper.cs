using Microsoft.Data.Sqlite;

namespace HabitLoggerMvc.Helpers;

public static class SqliteExceptionHelper
{
    #region Normal Codes

    public const int SQLITE_ERROR = 1;
    public const int SQLITE_BUSY = 5;
    public const int SQLITE_LOCKED = 6;
    public const int SQLITE_READONLY = 8;
    public const int SQLITE_IOERR = 10;
    public const int SQLITE_CORRUPT = 11;
    public const int SQLITE_FULL = 13;
    public const int SQLITE_CANTOPEN = 14;
    public const int SQLITE_PERM = 3;
    public const int SQLITE_AUTH = 23;
    public const int SQLITE_NOTADB = 26;
    public const int SQLITE_CONSTRAINT = 19;

    #endregion
    #region Extended Codes
    public const int SQLITE_CONSTRAINT_UNIQUE = 2067;
    public const int SQLITE_CONSTRAINT_PRIMARYKEY = 1555;
    public const int SQLITE_CONSTRAINT_FOREIGNKEY = 787;
    public const int SQLITE_CONSTRAINT_NOTNULL = 1299;
    #endregion

    public static string BuildUserErrorMessage(this SqliteException exception)
    {
        int code = exception.SqliteErrorCode;
        int extended = exception.SqliteExtendedErrorCode;

        return code switch
        {
            SQLITE_CONSTRAINT => extended switch
            {
                SQLITE_CONSTRAINT_UNIQUE or SQLITE_CONSTRAINT_PRIMARYKEY =>
                    "That value already exists. Please choose a different one.",

                SQLITE_CONSTRAINT_NOTNULL => "Please fill in all required fields and try again.",

                SQLITE_CONSTRAINT_FOREIGNKEY =>
                    "This item can’t be saved because it references something that doesn’t exist anymore. Refresh and try again.",

                _ => "We couldn’t save your changes because some data is invalid or conflicts with an existing record."
            },
            SQLITE_BUSY or SQLITE_LOCKED => "The database is busy right now. Please try again in a moment.",
            SQLITE_READONLY => "Saving is currently disabled (database is read-only). Please try again later.",
            SQLITE_FULL => "Storage is full, so your changes can’t be saved. Free up space and try again.",
            SQLITE_CANTOPEN or SQLITE_PERM or SQLITE_AUTH =>
                "The app can’t access its database right now. Please try again later.",
            SQLITE_CORRUPT or SQLITE_NOTADB => "The database appears to be damaged. Please contact support.",
            SQLITE_IOERR => "A storage error occurred while saving. Please try again.",
            _ => "Something went wrong while handling the database. Please try again."
        };
    }
}
