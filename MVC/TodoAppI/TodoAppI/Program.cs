using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using TodoAppI.Data;
using TodoAppI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("todolist"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<TodoContext>().Database.EnsureCreated();
}

var todoRoute = app.MapGroup("/todos");
todoRoute.MapGet("", async (TodoContext context) => await context.TodoItems.ToArrayAsync() ).WithName("GetTodos").WithOpenApi();

todoRoute.MapPost("", async (TodoContext context, TodoItem todoItem) =>
{
    if(string.IsNullOrEmpty(todoItem.Name))
        return Results.BadRequest();

    context.TodoItems.Add(todoItem);
    await context.SaveChangesAsync();

    return Results.NoContent();
}).WithName("CreateTodo").WithOpenApi();

todoRoute.MapPut("{id:int}", async (TodoContext context, int id, TodoItem item) =>
{
    var todoItem = await context.TodoItems.FindAsync(id);
    if(todoItem == null)
        return Results.NotFound();

    if (!string.IsNullOrEmpty(item.Name))
        todoItem.Name = item.Name;

    if (item.Completed != null)
        todoItem.Completed = item.Completed;

    context.TodoItems.Update(todoItem);
    await context.SaveChangesAsync();

    return Results.NoContent();
}).WithName("UpdateTodo").WithOpenApi();



app.Run();