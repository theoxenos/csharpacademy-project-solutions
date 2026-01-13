using TodoAppI.DTOs;

namespace TodoAppI.Services;

public interface ITodoService
{
    Task<IEnumerable<TodoItemDto>> GetAllTodosAsync();
    Task<TodoItemDto?> GetTodoByIdAsync(int id);
    Task<TodoItemDto> CreateTodoAsync(CreateTodoDto createDto);
    Task<bool> UpdateTodoAsync(int id, UpdateTodoDto updateDto);
    Task<bool> DeleteTodoAsync(int id);
}
