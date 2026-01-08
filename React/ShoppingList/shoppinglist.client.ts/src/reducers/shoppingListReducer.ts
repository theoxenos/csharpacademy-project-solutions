import {createSlice, type PayloadAction} from "@reduxjs/toolkit";

import itemService from "../services/itemService";
import shoppingListService from "../services/shoppingListService";
import {selectList} from "./uiReducer";
import type {Item, ShoppingList} from "../types";
import type {AppDispatch, RootState} from "../store";

const initialState: ShoppingList[] = [];

const shoppingListSlice = createSlice({
    name: 'shoppingList',
    initialState,
    reducers: {
        setShoppingLists: (_state, action: PayloadAction<ShoppingList[]>) => {
            return action.payload;
        },
        addShoppingList: (state, action: PayloadAction<ShoppingList>) => {
            state.push(action.payload);
        },
        updateShoppingList: (state, action: PayloadAction<{shoppingListId: number, updatedShoppingList: ShoppingList}>) => {
            const {shoppingListId, updatedShoppingList} = action.payload;
            const shoppingListIndex = state.findIndex(list => list.id === shoppingListId);
            if (shoppingListIndex !== -1) {
                state[shoppingListIndex] = updatedShoppingList;
            }
        },
        removeShoppingList: (state, action: PayloadAction<number>) => {
            const index = state.findIndex(list => list.id === action.payload);
            if (index !== -1) {
                state.splice(index, 1);
            }
        },
        updateItem: (state, action: PayloadAction<{shoppingListId: number, updatedItem: Item}>) => {
            const {shoppingListId, updatedItem} = action.payload;
            const shoppingListItem = state.find(list => list.id === shoppingListId);
            if (shoppingListItem) {
                const itemIndex = shoppingListItem.items.findIndex(item => item.id === updatedItem.id);
                if (itemIndex !== -1) {
                    shoppingListItem.items[itemIndex] = updatedItem;
                }
            }
        }
    }
});

const {setShoppingLists, addShoppingList, updateItem, removeShoppingList} = shoppingListSlice.actions;

export const initialiseShoppingLists = () => {
    return async (dispatch: AppDispatch) => {
        const shoppingLists: ShoppingList[] = await shoppingListService.getAllShoppingLists();
        dispatch(setShoppingLists(shoppingLists));
    };
};

export const createShoppingList = (newShoppingList: ShoppingList) => {
    return async (dispatch: AppDispatch) => {
        const shoppingList: ShoppingList = await shoppingListService.createShoppingList(newShoppingList);
        dispatch(addShoppingList(shoppingList));
        dispatch(selectList(shoppingList.id));
    };
};

let updateItemTimerId: ReturnType<typeof setTimeout> | null = null;

export const updateShoppingListItem = (shoppingListId: number, item: Item) => {
    return (dispatch: AppDispatch) => {
        dispatch(updateItem({shoppingListId, updatedItem: item}));

        if (updateItemTimerId) {
            clearTimeout(updateItemTimerId);
        }

        updateItemTimerId = setTimeout(async () => {
            try {
                const updatedItem: Item = await itemService.updateItem(item);
                dispatch(updateItem({shoppingListId, updatedItem}));
            } catch (error) {
                console.error("Failed to update item:", error);
            }
        }, 500);
    };
};

export const deleteShoppingList = (shoppingListId: number) => {
    return async (dispatch: AppDispatch) => {
        await shoppingListService.deleteShoppingList(shoppingListId);

        dispatch(removeShoppingList(shoppingListId));
    };
};

export const createItem = (shoppingListId: number, name: string) => {
    return async (dispatch: AppDispatch, getState: () => RootState) => {
        const newItem: Item = await itemService.createItem(shoppingListId, name);
        const list = getState().shoppingLists.find(list => list.id === shoppingListId);
        if (list) {
            const updatedShoppingList = {...list, items: [...list.items, newItem]};
            dispatch(updateShoppingList({shoppingListId, updatedShoppingList}));
        }
    };
};

export const {updateShoppingList} = shoppingListSlice.actions;
export default shoppingListSlice.reducer;

