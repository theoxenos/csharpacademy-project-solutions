const createShoppingList = async (newShoppingList) => {
    const response = await fetch('/api/shoppinglists/', {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(newShoppingList),
    });
    
    if(!response.ok) {
        throw new Error(response.error);
    }
    
    return await response.json();
};

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

const deleteShoppingList = async (id) => {
    const response = await fetch(`/api/shoppinglists/${id}`, {
        method: 'DELETE',
    });
    
    if(!response.ok) {
        throw new Error('Failed to delete shopping list');
    }
    
    // return await response.json();
};

export default {createShoppingList, getAllShoppingLists, updateShoppingList, deleteShoppingList};