import type {Item} from "../types.ts";

const createItem = async (listId: number, name: string) => {
    const response = await fetch(`/api/items`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({shoppingListId: listId, name}),
    });
    
    if(!response.ok) {
        throw new Error('Failed to add item');
    }
    
    return await response.json() as Item;
};

const updateItem = async (updatedItem: Item) => {
    const response = await fetch(`/api/items/${updatedItem.id.toString()}`, {
        method: 'PUT',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(updatedItem),
    });
    
    if(!response.ok) {
        throw new Error('Failed to update item');
    }
    
    return await response.json() as Item;
};

export default {createItem, updateItem};