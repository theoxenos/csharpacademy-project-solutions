using HabitLoggerMvc.Data;
using HabitLoggerMvc.Models;
using HabitLoggerMvc.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddTransient<HabitLoggerContext>();

builder.Services.AddTransient<IRepository<Habit>, HabitRepository>();
builder.Services.AddTransient<IHabitUnitRepository, HabitUnitRepository>();
builder.Services.AddTransient<IHabitLogRepository, HabitLogRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// app.UseAuthorization();

app.MapRazorPages();

await app.Services.GetRequiredService<HabitLoggerContext>().Init();

app.Run();