import {createSlice, type PayloadAction} from "@reduxjs/toolkit";
import authService from "../services/authService.js";
import type {User} from "../types.ts";
import type {AppDispatch} from "../store.ts";

const initialState: User = {} as User;

const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        setUser: (_state, action: PayloadAction<User>) => {
            return action.payload;
        }
    }
});

const {setUser} = userSlice.actions;

export const loginUser = (email: string, password: string) => {
    return async (dispatch: AppDispatch) => {
        const user = await authService.loginUser(email, password);
        dispatch(setUser(user));
    };
};

export default userSlice.reducer;

