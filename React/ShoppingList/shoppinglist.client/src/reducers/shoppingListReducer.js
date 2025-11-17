import {createSlice} from "@reduxjs/toolkit";

import shoppingListService from "../services/shoppingListService";

const shoppingListSlice = createSlice({
    name: 'shoppingList',
    initialState: [],
    reducers: {
        setShoppingLists: (state, action) => {
            return action.payload;
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

export default shoppingListSlice.reducer;

