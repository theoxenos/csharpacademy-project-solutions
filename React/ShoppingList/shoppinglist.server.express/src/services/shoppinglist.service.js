import ShoppingList from '../models/shoppinglist.model.js';

export const getAllShoppingLists = async () => {
    return ShoppingList.find({});
};