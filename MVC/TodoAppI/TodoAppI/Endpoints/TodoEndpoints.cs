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
            .WithName("GetTodos");

        todoRoute.MapGet("{id:int}", GetTodoById)
            .WithName("GetTodoById");

        todoRoute.MapPost("", CreateTodo)
            .WithName("CreateTodo");

        todoRoute.MapPut("{id:int}", UpdateTodo)
            .WithName("UpdateTodo");

        todoRoute.MapDelete("{id:int}", DeleteTodo)
            .WithName("DeleteTodo");
    }

    private static async Task<Results<Ok<TodoItemDto>, NotFound<string>>> GetTodoById(ITodoService todoService, int id)
    {
        var todo = await todoService.GetTodoByIdAsync(id);
        if (todo == null)
            return TypedResults.NotFound($"Todo with id {id} not found");

        return TypedResults.Ok(todo);
    }

    private static async Task<Ok<PagedResponse<TodoItemDto>>> GetTodos(ITodoService todoService,
        [AsParameters] PaginationRequest request)
    {
        var todos = await todoService.GetAllTodosAsync(request.PageNumber, request.PageSize);
        return TypedResults.Ok(todos);
    }

    private static async Task<Results<NoContent, BadRequest<string>>> CreateTodo(ITodoService todoService,
        CreateTodoDto createDto)
    {
        if (string.IsNullOrWhiteSpace(createDto.Name))
            return TypedResults.BadRequest("Name is required");

        await todoService.CreateTodoAsync(createDto);
        return TypedResults.NoContent();
    }

    private static async Task<Results<NoContent, BadRequest<string>, NotFound<string>>> UpdateTodo(
        ITodoService todoService, int id,
        UpdateTodoDto updateDto)
    {
        if (updateDto.Name != null && string.IsNullOrWhiteSpace(updateDto.Name))
            return TypedResults.BadRequest("Name is required");

        var success = await todoService.UpdateTodoAsync(id, updateDto);
        return success ? TypedResults.NoContent() : TypedResults.NotFound($"Note with id {id} not found");
    }

    private static async Task<Results<NoContent, NotFound<string>>> DeleteTodo(ITodoService todoService, int id)
    {
        var success = await todoService.DeleteTodoAsync(id);
        return success ? TypedResults.NoContent() : TypedResults.NotFound($"Note with id {id} not found");
    }
}