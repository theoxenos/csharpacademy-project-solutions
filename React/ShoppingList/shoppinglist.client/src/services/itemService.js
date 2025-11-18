const updateItem = async (updatedItem) => {
    const response = await fetch(`/api/items/${updatedItem.id}`, {
        method: 'PUT',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(updatedItem),
    });
    
    if(!response.ok) {
        throw new Error('Failed to update item');
    }
    
    return await response.json();
};

export default {updateItem};