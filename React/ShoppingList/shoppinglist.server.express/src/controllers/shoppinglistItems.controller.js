import * as shoppingListItemService from '../services/shoppinglistItem.service.js';

export const createShoppingListItem = async (req, res, next) => {
    try {
        const result = await shoppingListItemService.createShoppingListItem(req.body);
        res.status(201).send(result);
    } catch (error) {
        next(error);
    }
};

export const updateShoppingListItem = async (req, res, next) => {
    if (req.params.id !== req.body.id) {
        return res.status(400).send({error: 'ID does not match'});
    }

    try {
        const result = await shoppingListItemService.updateShoppingListItem(req.body);
        res.status(200).send(result);
    } catch (error) {
        next(error);
    }
};