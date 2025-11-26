import {configureStore} from "@reduxjs/toolkit";
import shoppingListReducer from "./reducers/shoppingListReducer.js";
import uiReducer from "./reducers/uiReducer.js";
import userReducer from "./reducers/userReducer.js";

const store = configureStore({
    reducer: {
        shoppingLists: shoppingListReducer,
        user: userReducer,
        ui: uiReducer
    }
});
export default store;