const getAllTodos = async () => {
    const response = await fetch('/todos');
    if(!response.ok) throw new Error('Something went wrong');
    return response.json()
}

const deleteTodoById = async (id) => {
    const response = await fetch(`/todos/${id}`, {method: 'DELETE'});
    if(!response.ok) throw new Error('Something went wrong');
}

export default {getAllTodos, deleteTodoById};