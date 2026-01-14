const getAllTodos = async () => {
    const response = await fetch('/todos');
    if(!response.ok) throw new Error('Something went wrong');
    return response.json()
}

export default {getAllTodos};