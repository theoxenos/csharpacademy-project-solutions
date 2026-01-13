using Microsoft.EntityFrameworkCore;
using TodoAppI.Data;
using TodoAppI.DTOs;
using TodoAppI.Models;

namespace TodoAppI.Services;

public class TodoService(TodoContext context) : ITodoService
{
    public async Task<IEnumerable<TodoItemDto>> GetAllTodosAsync()
    {
        return await context.TodoItems
            .Select(t => new TodoItemDto(t.Id, t.Name, t.Completed))
            .ToListAsync();
    }

    public async Task<TodoItemDto?> GetTodoByIdAsync(int id)
    {
        var todo = await context.TodoItems.FindAsync(id);
        return todo == null ? null : new TodoItemDto(todo.Id, todo.Name, todo.Completed);
    }

    public async Task<TodoItemDto> CreateTodoAsync(CreateTodoDto createDto)
    {
        var todo = new TodoItem
        {
            Name = createDto.Name,
            Completed = false
        };

        context.TodoItems.Add(todo);
        await context.SaveChangesAsync();

        return new TodoItemDto(todo.Id, todo.Name, todo.Completed);
    }

    public async Task<bool> UpdateTodoAsync(int id, UpdateTodoDto updateDto)
    {
        var todo = await context.TodoItems.FindAsync(id);
        if (todo == null) return false;

        if (updateDto.Name != null) todo.Name = updateDto.Name;
        if (updateDto.Completed.HasValue) todo.Completed = updateDto.Completed.Value;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        var todo = await context.TodoItems.FindAsync(id);
        if (todo == null) return false;

        context.TodoItems.Remove(todo);
        await context.SaveChangesAsync();
        return true;
    }
}
