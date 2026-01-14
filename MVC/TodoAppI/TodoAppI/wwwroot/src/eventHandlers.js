import todosApiService from "./todosApiService.js";

export const deleteTodoItemListener = async (id) => {
    try {
        await todosApiService.deleteTodoById(id);
    } catch (error) {
        throw error;
    }
}

export const updateTodoItemListener = async (todo) => {
    try {
        await todosApiService.updateTodo(todo);
    } catch (error) {
        throw error;
    }
}