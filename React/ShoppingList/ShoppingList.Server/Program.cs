using ShoppingList.Server.Database;
using ShoppingList.Server.Repositories;
using ShoppingList.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

builder.Services.AddSingleton<IDatabaseConnectionFactory, SqliteConnectionFactory>();
builder.Services.AddSingleton<DatabaseInitialiser>();

builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IShoppingListRepository, ShoppingListRepository>();

builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IShoppingListService, ShoppingListService>();

var app = builder.Build();

var databaseInitialiser = app.Services.GetRequiredService<DatabaseInitialiser>();
await databaseInitialiser.InitialiseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();