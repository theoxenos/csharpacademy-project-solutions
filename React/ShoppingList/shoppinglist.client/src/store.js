import {configureStore} from "@reduxjs/toolkit";
import shoppingListReducer from "./reducers/shoppingListReducer.js";

const store = configureStore({
    reducer: {
        shoppingLists: shoppingListReducer,
    },
});
export default store;