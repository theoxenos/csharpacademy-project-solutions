import {PagedResponse, Todo, TodoUpsert} from '../types.js';

const getAllTodos = async (pageNumber: number, pageSize: number): Promise<PagedResponse<Todo>> => {
    const response = await fetch(`/todos?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    if(!response.ok) throw new Error('Something went wrong when fetching todos');
    return response.json()
}

const deleteTodoById = async (id: number): Promise<void> => {
    const response = await fetch(`/todos/${id}`, {method: 'DELETE'});
    if (!response.ok) {
        const errorMessage = await response.text();
        throw new Error(`Something went wrong when deleting todo: ${errorMessage}`);
    }
}

const createTodo = async (todo: TodoUpsert): Promise<void> => {
    const {name} = todo;
    const response = await fetch('/todos', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({name})
    });
    if (!response.ok) {
        const errorMessage = await response.text();
        throw new Error(`Something went wrong when creating todo: ${errorMessage}`);
    }
}

const updateTodo = async (todo: TodoUpsert): Promise<void> => {
    const {id, name, completed} = todo;
    const response = await fetch(`/todos/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({name, completed})
    });
    if (!response.ok) {
        const errorMessage = await response.text();
        throw new Error(`Something went wrong when updating todo: ${errorMessage}`);
    }
}

export default {getAllTodos, deleteTodoById, createTodo, updateTodo};