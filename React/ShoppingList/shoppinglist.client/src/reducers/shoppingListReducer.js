import {createSlice} from "@reduxjs/toolkit";

import shoppingListService from "../services/shoppingListService";

const shoppingListSlice = createSlice({
    name: 'shoppingList',
    initialState: [],
    reducers: {
        setShoppingLists: (state, action) => {
            return action.payload;
        },
        toggleItemChecked: (state, action) => {
            const {shoppingListId, itemId} = action.payload;
            const shoppingListItem = state.find(list => list.id === shoppingListId);
            const item = shoppingListItem.items.find(item => item.id === itemId);
            item.isChecked = !item.isChecked;
        }
    }
});

const {setShoppingLists} = shoppingListSlice.actions;

export const initialiseShoppingLists = () => {
    return async (dispatch) => {
        const shoppingLists =  await shoppingListService.getAllShoppingLists();
        dispatch(setShoppingLists(shoppingLists));
    }
};

export const {toggleItemChecked} = shoppingListSlice.actions;
export default shoppingListSlice.reducer;

