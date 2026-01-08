import {createSlice, type PayloadAction} from "@reduxjs/toolkit";
import type {UiState} from "../types.ts";

const initialState: UiState = {
    loading: false,
    selectedListId: null
};

const uiSlice = createSlice({
    name: 'ui',
    initialState,
    reducers: {
        selectList: (state, action: PayloadAction<number | null>) => {
            state.selectedListId = action.payload;
        }
    }
});

export const {selectList} = uiSlice.actions;
export default uiSlice.reducer;