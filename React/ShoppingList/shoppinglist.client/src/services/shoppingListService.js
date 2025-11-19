const getAllShoppingLists = async () => {
    const response = await fetch('/api/shoppinglists');
    console.log('fetching lists');
    if(!response.ok) {
        throw new Error('Failed to fetch shopping lists');
    }
    
    return await response.json();
};

const updateShoppingList = async (id, name) => {
    const response = await fetch(`/api/shoppinglists/${id}`, {
        method: 'PUT',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({id, name}),
    });
    
    if(!response.ok) {
        throw new Error('Failed to update shopping list');
    }
    
    return await response.json();
};

export default {getAllShoppingLists, updateShoppingList};