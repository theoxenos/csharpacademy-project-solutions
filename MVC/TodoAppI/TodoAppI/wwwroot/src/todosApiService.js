const getAllTodos = async () => {
    const response = await fetch('/todos');
    if(!response.ok) throw new Error('Something went wrong when fetching todos');
    return response.json()
}

const deleteTodoById = async (id) => {
    const response = await fetch(`/todos/${id}`, {method: 'DELETE'});
    if(!response.ok) throw new Error('Something went wrong when deleting todo');
}

const createTodo = async (todo) => {
    const {name} = todo;
    const response = await fetch('/todos', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({name})
    });
    if(!response.ok) throw new Error('Something went wrong when creating todo');
}

const updateTodo = async (todo) => {
    const {id, name, completed} = todo;
    const response = await fetch(`/todos/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({name, completed})
    });
    if(!response.ok) throw new Error('Something went wrong when updating todo');
}

export default {getAllTodos, deleteTodoById, createTodo, updateTodo};