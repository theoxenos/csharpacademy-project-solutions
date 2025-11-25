import * as shoppingListService from '../services/shoppinglist.service.js';

export const getAllShoppingLists = async (req, res, next) => {
    try {
        const shoppingLists = await shoppingListService.getAllShoppingLists();
        res.json(shoppingLists);
    } catch (error) {
        next(error);
    }
};

export const addShoppingList = async (req, res, next) => {
    try {
        const shoppingListName = req.body.name;
        const response = await shoppingListService.addShoppingList(shoppingListName);
        res.status(201).send(response);
    } catch (error) {
        next(error);
    }
};

export const deleteShoppingList = async (req, res, next) => {
    try {
        const response = await shoppingListService.deleteShoppingList(req.params.id);
        
        if(response === null) {
            res.status(404).send({error: `Could not delete shoppinglist with ID ${req.params.id}`});
        }
        
        res.status(200).send(response);
    } catch (error) {
        next(error);
    }
};

export const updateShoppingList = async (req, res, next) => {
    if(req.params.id !== req.body.id) {
        res.status(404).send({error: `Could not update shopping list`});
    }
    
    try {
        const response = await shoppingListService.updateShoppingList(req.body);
        res.status(200).send(response);
    } catch (error) {
        next(error);
    }
};