import {createSlice} from "@reduxjs/toolkit";

const initialState = {
    loading: false,
    selectedListId: null
};

const uiSlice = createSlice({
    name: 'ui',
    initialState,
    reducers: {
        selectList: (state, action) => {
            state.selectedListId = action.payload;
        }
    }
});

export const {selectList} = uiSlice.actions;
export default uiSlice.reducer;