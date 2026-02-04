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

builder.Services.AddScoped<DatabaseService>();

builder.Services.AddScoped<IIngredientsRepository, IngredientsRepository>();

builder.Services.AddScoped<IngredientService>();
builder.Services.AddScoped<RecipeService>();

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
    var databaseService = resource.ServiceProvider.GetService<DatabaseService>();
    if (databaseService == null) return;
    await databaseService.CreateDatabase();
}

app.Run();