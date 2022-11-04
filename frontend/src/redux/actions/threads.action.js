import {threadsConstants} from "../actiontypes";
import {LikeThreadClient, ThreadClient} from "../../api";
import {store} from "../store";
import {findThread, findThreadInStoreState} from "../state.helper";
import {mapThreadsToViewModels} from "../threadViewModel";
import {commentActions} from "./comment.actions";

export const threadActions = {
    updateThreads,
    toggleThreadLike,
    toggleCommentSection
}

function updateThreads() {
    return dispatch => {
        dispatch(request());

        let client = new ThreadClient();
        client.list().then(
            result => {
                const threads = mapThreadsToViewModels(result.threads);
                dispatch(success(threads));
            },
            error => {

            }
        )
    }

    function request() {
        return {type: threadsConstants.THREADS_REQUESTED}
    }

    function success(threads) {
        return {type: threadsConstants.THREADS_RECEIVED, threads}
    }
}

function toggleThreadLike(threadId) {
    return (dispatch, getState) => {
        const state = getState();
        const thread = findThreadInStoreState(threadId, state);
        if (thread.isChangingLikeStatus) return;

        dispatch(request(threadId));

        if (thread.likedByCurrentUser) {
            let client = new LikeThreadClient();
            client.unlike(thread.id).then(
                () => {
                    dispatch(unlike(threadId));
                }
            );
        } else {
            let client = new LikeThreadClient();
            client.like(thread.id).then(
                () => {
                    dispatch(like(threadId));
                }
            );
        }
    }

    function request() {
        return {type: threadsConstants.THREAD_LIKE_TOGGLE_REQUESTED, threadId}
    }

    function like(threadId) {
        return {type: threadsConstants.THREAD_LIKED, threadId}
    }

    function unlike(threadId) {
        return {type: threadsConstants.THREAD_UNLIKED, threadId}
    }
}

function toggleCommentSection(threadId) {
    return (dispatch, getState) => {
        // Only fetch comments if comment section is going to be toggled on.
        const state = getState();
        if (state.threadsReducer.threadsById[threadId].showingCommentSection === false){
            dispatch(commentActions.updateCommentsForThread(threadId));
        }

        dispatch(toggle(threadId));
    }

    function toggle() {
        return {type: threadsConstants.THREAD_COMMENTS_TOGGLED, threadId};
    }
}