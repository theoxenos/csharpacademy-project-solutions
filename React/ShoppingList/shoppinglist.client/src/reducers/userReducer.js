import {createSlice} from "@reduxjs/toolkit";
import authService from "../services/authService.js";

const userSlice = createSlice({
    name: 'user',
    initialState: {},
    reducers: {
        setUser: (state, action) => {
            state.user = action.payload;
        }
    }
});

const {setUser} = userSlice.actions;

export const loginUser = (email, password) => {
    return async (dispatch) => {
        const user = await authService.loginUser(email, password);
        dispatch(setUser(user));
    };
};

export default userSlice.reducer;

