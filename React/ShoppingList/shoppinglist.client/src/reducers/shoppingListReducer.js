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
        updateShoppingList: (state, action) => {
          const {shoppingListId, updatedShoppingList} = action.payload;
          const shoppingListIndex = state.findIndex(list => list.id === shoppingListId);
          if (shoppingListIndex !== -1) {
              state[shoppingListIndex] = updatedShoppingList;
          }
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

export const createItem = (shoppingListId, name) => {
    return async (dispatch, getState) => {
        const newItem = await itemService.createItem(shoppingListId, name);
        const list = getState().shoppingLists.find(list => list.id === shoppingListId);
        const updatedShoppingList = {...list, items: [...list.items, newItem]};
        
        dispatch(updateShoppingList({shoppingListId, updatedShoppingList}));
    }
};

export const {updateShoppingList} = shoppingListSlice.actions;
export default shoppingListSlice.reducer;

