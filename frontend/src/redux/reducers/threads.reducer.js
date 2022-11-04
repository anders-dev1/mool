import {threadsConstants} from "../actiontypes";
import {createReducer} from "@reduxjs/toolkit";
import {findThreadInThreadState} from "../state.helper";

export const threadsReducer = createReducer(
    {
        requestingThreads: false,
        threadsById: {},
        threadIds: []
    },
    (builder) => {
        builder.addCase(threadsConstants.THREADS_REQUESTED, (state) => {
            state.requestingThreads = true;
        });
        builder.addCase(threadsConstants.THREADS_RECEIVED, (state, action) => {
            state.requestingThreads = false;

            action.threads.forEach(function (thread) {
                state.threadsById[thread.id] = thread;
            });
            state.threadIds = action.threads.map(thread => thread.id);
        });
        builder.addCase(threadsConstants.THREAD_LIKE_TOGGLE_REQUESTED, (state, action) => {
            const thread = findThreadInThreadState(action.threadId, state);
            thread.isChangingLikeStatus = true;
        });
        builder.addCase(threadsConstants.THREAD_LIKED, (state, action) => {
            const thread = findThreadInThreadState(action.threadId, state);
            thread.isChangingLikeStatus = false;
            thread.likedByCurrentUser = true;
            thread.likes++;
        });
        builder.addCase(threadsConstants.THREAD_UNLIKED, (state, action) => {
            const thread = findThreadInThreadState(action.threadId, state);
            thread.isChangingLikeStatus = false;
            thread.likedByCurrentUser = false;
            thread.likes--;
        });
        builder.addCase(threadsConstants.THREAD_COMMENTS_TOGGLED, (state, action) => {
            const thread = findThreadInThreadState(action.threadId, state);
            thread.showingCommentSection = !thread.showingCommentSection;
        });
        builder.addCase(threadsConstants.THREAD_COMMENT_ORDER_CHANGED, (state, action) => {
            const thread = findThreadInThreadState(action.threadId, state);
            thread.commentOrder = action.order;
        });
        builder.addCase(threadsConstants.THREAD_COMMENTS_REQUESTED, (state, action) => {
            const thread = findThreadInThreadState(action.threadId, state);
            thread.fetchingComments = true;
        });
        builder.addCase(threadsConstants.THREAD_COMMENTS_RECEIVED, (state, action) => {
            const thread = findThreadInThreadState(action.threadId, state);
            thread.fetchingComments = false;
        });
        builder.addCase(threadsConstants.THREAD_COMMENT_ADD_REQUESTED, (state, action) => {
            const thread = findThreadInThreadState(action.threadId, state);
            thread.postingComment = true;
        });
        builder.addCase(threadsConstants.THREAD_COMMENT_ADDED, (state, action) => {
            const thread = findThreadInThreadState(action.threadId, state);
            thread.postingComment = false;
            thread.numberOfComments++;
        });
    }
)

