import {configureStore} from "@reduxjs/toolkit";
import shoppingListReducer from "./reducers/shoppingListReducer.js";
import uiReducer from "./reducers/uiReducer.js";

const store = configureStore({
    reducer: {
        shoppingLists: shoppingListReducer,
        ui: uiReducer
    }
});
export default store;