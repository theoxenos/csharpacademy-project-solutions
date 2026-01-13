using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
builder.Services.AddRazorPages();
// builder.Services.AddExceptionHandler<ExceptionHandler>();

string sqliteConnectionString =
    builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=habitlogger.db";

builder.Services.AddDbContext<HabitLoggerContext>(options =>
    options.UseSqlite(sqliteConnectionString));

builder.Services.AddScoped<IRepository<Habit>, HabitRepository>();
builder.Services.AddScoped<IHabitUnitRepository, HabitUnitRepository>();
builder.Services.AddScoped<IHabitLogRepository, HabitLogRepository>();

WebApplication app = builder.Build();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// app.UseAuthorization();

app.MapRazorPages();

using (IServiceScope scope = app.Services.CreateScope())
{
    HabitLoggerContext context = scope.ServiceProvider.GetRequiredService<HabitLoggerContext>();

    string? connectionString = context.Database.GetConnectionString();
    SqliteConnectionStringBuilder builderSqlite = new(connectionString);
    string databaseFile = builderSqlite.DataSource;
    if (!string.IsNullOrEmpty(databaseFile) && databaseFile != ":memory:")
    {
        string? directory = Path.GetDirectoryName(databaseFile);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    context.Database.EnsureCreated();
}

app.Run();
