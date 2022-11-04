import rootReducer from "./reducers/root.reducer";
import {configureStore} from "@reduxjs/toolkit";

export const setupStore = preloadedState => {
    return configureStore({
        reducer: rootReducer,
        preloadedState
    });
}

export const store = setupStore();