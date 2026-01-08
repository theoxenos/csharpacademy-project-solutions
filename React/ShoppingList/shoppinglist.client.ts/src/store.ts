import {configureStore} from "@reduxjs/toolkit";
import shoppingListReducer from "./reducers/shoppingListReducer";
import uiReducer from "./reducers/uiReducer";
import userReducer from "./reducers/userReducer";

const store = configureStore({
    reducer: {
        shoppingLists: shoppingListReducer,
        user: userReducer,
        ui: uiReducer
    }
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export default store;