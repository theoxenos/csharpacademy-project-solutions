const getAllShoppingLists = async () => {
    const response = await fetch('/api/shoppinglists');
    console.log('fetching lists');
    if(!response.ok) {
        throw new Error('Failed to fetch shopping lists');
    }
    
    return await response.json();
};

export default {getAllShoppingLists};