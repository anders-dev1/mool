export const findThreadInStoreState = (threadId, state) => state.threadsReducer.threadsById[threadId];
export const findThreadInThreadState = (threadId, state) => state.threadsById[threadId];