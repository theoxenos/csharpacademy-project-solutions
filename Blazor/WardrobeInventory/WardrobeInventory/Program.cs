using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using WardrobeInventory;
using WardrobeInventory.Data;
using WardrobeInventory.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddDbContext<WardrobeContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ImageService>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WardrobeContext>();
    context.Database.EnsureCreated();
}

await app.RunAsync();