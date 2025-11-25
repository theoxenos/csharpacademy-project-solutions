import * as shoppingListService from '../services/shoppinglist.service.js';

export const getAllShoppingLists = async (req, res, next) => {
    try {
        const shoppingLists = await shoppingListService.getAllShoppingLists();
        res.json(shoppingLists);
    } catch (error) {
        next(error);
    }
};