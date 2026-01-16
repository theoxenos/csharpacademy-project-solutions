using TodoAppI.DTOs;

namespace TodoAppI.Services;

public interface ITodoService
{
    Task<PagedResponse<TodoItemDto>> GetAllTodosAsync(int page = 1, int pageSize = 5);
    Task<TodoItemDto?> GetTodoByIdAsync(int id);
    Task<TodoItemDto> CreateTodoAsync(CreateTodoDto createDto);
    Task<bool> UpdateTodoAsync(int id, UpdateTodoDto updateDto);
    Task<bool> DeleteTodoAsync(int id);
}