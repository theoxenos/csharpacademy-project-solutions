using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;
using WardrobeInventory;
using WardrobeInventory.Data;
using WardrobeInventory.Models;
using WardrobeInventory.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSqliteWasmDbContextFactory<WardrobeContext>(opt => opt.UseSqlite("Data Source=wardrobe.db"));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<WardrobeService>();

await builder.Build().RunAsync();
