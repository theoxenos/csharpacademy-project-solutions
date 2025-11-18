import {createSlice} from "@reduxjs/toolkit";

import itemService from "../services/itemService";
import shoppingListService from "../services/shoppingListService";

const shoppingListSlice = createSlice({
    name: 'shoppingList',
    initialState: [],
    reducers: {
        setShoppingLists: (state, action) => {
            return action.payload;
        },
        updateItem: (state, action) => {
            const {shoppingListId, updatedItem} = action.payload;
            const shoppingListItem = state.find(list => list.id === shoppingListId);
            const itemIndex = shoppingListItem.items.findIndex(item => item.id === updatedItem.id);
            if (itemIndex !== -1) {
                shoppingListItem.items[itemIndex] = updatedItem;
            }
        }
    }
});

const {setShoppingLists, updateItem} = shoppingListSlice.actions;

export const initialiseShoppingLists = () => {
    return async (dispatch) => {
        const shoppingLists =  await shoppingListService.getAllShoppingLists();
        dispatch(setShoppingLists(shoppingLists));
    }
};

export const updateShoppingListItem = (shoppingListId, item) => {
    return async (dispatch) => {
        const updatedItem = await itemService.updateItem(item);
        dispatch(updateItem({shoppingListId, updatedItem}));
    }
};

export const {toggleItemChecked} = shoppingListSlice.actions;
export default shoppingListSlice.reducer;

