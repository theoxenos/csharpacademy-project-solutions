namespace TodoAppI.DTOs;

public record TodoItemDto(int Id, string Name, bool Completed);

public record CreateTodoDto(string Name);

public record UpdateTodoDto(string? Name, bool? Completed);
