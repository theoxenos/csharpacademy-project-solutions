using HabitLoggerMvc.Data;
using HabitLoggerMvc.Middleware;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
builder.Services.AddRazorPages();

var sqlServerConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var sqliteConnectionString =
    builder.Configuration.GetConnectionString("SqliteConnection") ?? "Data Source=habitlogger.db";

if (string.IsNullOrEmpty(sqlServerConnectionString) && string.IsNullOrEmpty(sqliteConnectionString))
{
    throw new InvalidOperationException("No valid connection string found.");
}

if (!string.IsNullOrEmpty(sqlServerConnectionString))
{
    builder.Services.AddDbContext<HabitLoggerContext>(options =>
        options.UseSqlServer(sqlServerConnectionString));
}
else
{
    builder.Services.AddDbContext<HabitLoggerContext>(options =>
        options.UseSqlite(sqliteConnectionString));
}

builder.Services.AddScoped<IRepository<Habit>, HabitRepository>();
builder.Services.AddScoped<IHabitUnitRepository, HabitUnitRepository>();
builder.Services.AddScoped<IHabitLogRepository, HabitLogRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HabitLoggerContext>();

    if (context.Database.IsSqlite())
    {
        var connectionString = context.Database.GetConnectionString();
        if (connectionString != null)
        {
            var builderSqlite = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(connectionString);
            var databaseFile = builderSqlite.DataSource;
            if (!string.IsNullOrEmpty(databaseFile) && databaseFile != ":memory:")
            {
                var directory = Path.GetDirectoryName(databaseFile);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
        }
    }

    context.Database.EnsureCreated();
}

app.Run();
