import { combineReducers } from "@reduxjs/toolkit";
import { login } from "./session.reducer";
import { threadsReducer } from "./threads.reducer";
import {commentsReducer} from "./comments.reducer";

const rootReducer = combineReducers({
    login,
    threadsReducer,
    commentsReducer
})

export default rootReducer
