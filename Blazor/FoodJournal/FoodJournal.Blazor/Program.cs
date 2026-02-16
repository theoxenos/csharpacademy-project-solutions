using FoodJournal.Blazor.Features;
using FoodJournal.Blazor.Repositories;
using FoodJournal.Blazor.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IDatabaseService, DatabaseService>();

builder.Services.AddScoped<IIngredientsRepository, IngredientsRepository>();
builder.Services.AddScoped<IMealRepository, MealRepository>();

builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IIngredientReportsService, IngredientsReportsService>();
builder.Services.AddScoped<IMealService, MealService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var resource = app.Services.CreateScope())
{
    var databaseService = resource.ServiceProvider.GetService<IDatabaseService>();
    if (databaseService == null) throw new InvalidOperationException("Database service not found");
    await databaseService.CreateDatabase();
}

app.Run();