import type {ErrorResponse, ShoppingList} from "../types.ts";

const createShoppingList = async (newShoppingList: ShoppingList) => {
    const response = await fetch('/api/shoppinglists/', {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(newShoppingList),
    });
    
    if(!response.ok) {
        const {error} = await response.json() as ErrorResponse;
        throw new Error(error);
    }
    
    return await response.json() as ShoppingList;
};

const getAllShoppingLists = async () => {
    const response = await fetch('/api/shoppinglists');
    console.log('fetching lists');
    if(!response.ok) {
        const {error} = await response.json() as ErrorResponse;
        throw new Error(error);
    }
    
    return await response.json() as ShoppingList[];
};

const updateShoppingList = async (id: number, name: string) => {
    const response = await fetch(`/api/shoppinglists/${id.toString()}`, {
        method: 'PUT',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({id, name}),
    });
    
    if(!response.ok) {
        const {error} = await response.json() as ErrorResponse;
        throw new Error(error);
    }
    
    return await response.json() as ShoppingList;
};

const deleteShoppingList = async (id: number) => {
    const response = await fetch(`/api/shoppinglists/${id.toString()}`, {
        method: 'DELETE',
    });
    
    if(!response.ok) {
        throw new Error('Failed to delete shopping list');
    }
    
    // return await response.json();
};

export default {createShoppingList, getAllShoppingLists, updateShoppingList, deleteShoppingList};