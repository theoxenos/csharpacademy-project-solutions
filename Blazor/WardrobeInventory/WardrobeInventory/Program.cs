using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using WardrobeInventory;
using WardrobeInventory.Data;
using WardrobeInventory.Services;
using SqliteWasmHelper;
using WardrobeInventory.Model;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddSqliteWasmDbContextFactory<WardrobeContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient();

builder.Services.AddScoped<ImageService>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<ISqliteWasmDbContextFactory<WardrobeContext>>();
    var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

    await using var context = await factory.CreateDbContextAsync(); 
    var created = context.Database.EnsureCreated();
    if (created)
    {
        await context.WardrobeItems.AddRangeAsync(
            new WardrobeItem
            {
                Id = 1,
                Name = "My favourite shirt",
                Category = Category.Shirts,
                Brand = "Grandma",
                Size = Size.M,
                ImageData = await ConvertImageToBytes("item1.png", httpClientFactory)
            },
            new WardrobeItem
            {
                Id = 2,
                Name = "My favourite coat",
                Category = Category.Dress,
                Brand = "Prada",
                Size = Size.L,
                ImageData = await ConvertImageToBytes("item2.png", httpClientFactory)
            }
        );

        async Task<byte[]> ConvertImageToBytes(string imageName, IHttpClientFactory factory)
        {
            var httpClient = factory.CreateClient();
            return await httpClient.GetByteArrayAsync($"sample-images/{imageName}");
        }
    }
}

await app.RunAsync();