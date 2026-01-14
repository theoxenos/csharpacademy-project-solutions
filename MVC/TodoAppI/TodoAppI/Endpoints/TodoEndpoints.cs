using Microsoft.AspNetCore.Http.HttpResults;
using TodoAppI.DTOs;
using TodoAppI.Services;

namespace TodoAppI.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        var todoRoute = app.MapGroup("/todos");

        todoRoute.MapGet("", GetTodos)
            .WithName("GetTodos")
            .WithOpenApi();
        
        todoRoute.MapGet("{id:int}", GetTodoById)
            .WithName("GetTodoById")
            .WithOpenApi();

        todoRoute.MapPost("", CreateTodo)
            .WithName("CreateTodo")
            .WithOpenApi();

        todoRoute.MapPut("{id:int}", UpdateTodo)
            .WithName("UpdateTodo")
            .WithOpenApi();

        todoRoute.MapDelete("{id:int}", DeleteTodo)
            .WithName("DeleteTodo")
            .WithOpenApi();
    }

    private static async Task<Results<Ok<TodoItemDto>, NotFound>> GetTodoById(ITodoService todoService, int id)
    {
        var todo = await todoService.GetTodoByIdAsync(id);
        if (todo == null)
            return TypedResults.NotFound();
        
        return TypedResults.Ok(todo);
    }

    private static async Task<Ok<IEnumerable<TodoItemDto>>> GetTodos(ITodoService todoService)
    {
        var todos = await todoService.GetAllTodosAsync();
        return TypedResults.Ok(todos);
    }

    private static async Task<Results<NoContent, BadRequest>> CreateTodo(ITodoService todoService, CreateTodoDto createDto)
    {
        if (string.IsNullOrWhiteSpace(createDto.Name))
            return TypedResults.BadRequest();

        await todoService.CreateTodoAsync(createDto);
        return TypedResults.NoContent();
    }

    private static async Task<Results<NoContent, BadRequest, NotFound>> UpdateTodo(ITodoService todoService, int id, UpdateTodoDto updateDto)
    {
        if (updateDto.Name != null && string.IsNullOrWhiteSpace(updateDto.Name))
            return TypedResults.BadRequest();

        var success = await todoService.UpdateTodoAsync(id, updateDto);
        return success ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Results<NoContent, NotFound>> DeleteTodo(ITodoService todoService, int id)
    {
        var success = await todoService.DeleteTodoAsync(id);
        return success ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
