var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ShoppingList.Server.Infrastructure.DapperSetup.Register();

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new ShoppingList.Server.Web.Json.DateOnlyJsonConverter());
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DDD/Infrastructure registrations
builder.Services.AddSingleton<ShoppingList.Server.Abstractions.IDbConnectionFactory, ShoppingList.Server.Infrastructure.SqliteConnectionFactory>();
builder.Services.AddHostedService<ShoppingList.Server.Infrastructure.DatabaseInitializer>();

builder.Services.AddScoped<ShoppingList.Server.Domain.Repositories.IFoodRepository, ShoppingList.Server.Infrastructure.Repositories.DapperFoodRepository>();
builder.Services.AddScoped<ShoppingList.Server.Domain.Repositories.IMealRepository, ShoppingList.Server.Infrastructure.Repositories.DapperMealRepository>();

builder.Services.AddScoped<ShoppingList.Server.Application.Services.IFoodService, ShoppingList.Server.Application.Services.FoodService>();
builder.Services.AddScoped<ShoppingList.Server.Application.Services.IMealService, ShoppingList.Server.Application.Services.MealService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();