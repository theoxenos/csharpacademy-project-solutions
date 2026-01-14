export const fetchTodos = async () => {
    try {
        const response = await fetch('/todos');
        if (response.ok) {
            return await response.json();
        } else {
            throw new Error('Error fetching todos');
        }
    } catch (error) {
        throw error;
    }
};

export const createTodo = async (todo) => {
    const response = await fetch('todos', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(todo)
    });
    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || 'Error creating todo');
    }
    return response;
};

export const updateTodo = async (id, todo) => {
    const response = await fetch(`todos/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(todo)
    });
    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || 'Error updating todo');
    }
    return response;
};

export const deleteTodo = async (id) => {
    const response = await fetch(`todos/${id}`, {
        method: 'DELETE'
    });
    if (!response.ok) {
        throw new Error('Error deleting todo');
    }
    return response;
};
