import ShoppingList from '../models/shoppinglist.model.js';

export const getAllShoppingLists = async () => {
    return ShoppingList.find({});
};

export const addShoppingList = async (shoppingListName) => {
    const now = new Date().toISOString();
    return await ShoppingList.create({
        name: shoppingListName,
        createdAt: now,
        items: []
    });
};

export const deleteShoppingList = async (shoppingListId) => {
    return ShoppingList.findByIdAndDelete(shoppingListId);
};

export const updateShoppingList = async (updatedShoppingList) => {
    const {id, name} = updatedShoppingList;
    return ShoppingList.findByIdAndUpdate(id, {name}, {new: true});
};