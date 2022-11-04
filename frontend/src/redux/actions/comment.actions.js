import {commentsConstants, threadsConstants} from "../actiontypes";
import {CommentsClient, LikeCommentClient} from "../../api";
import {findThreadInStoreState} from "../state.helper";
import {mapCommentModelToViewModel} from "../commentViewModel";

export const commentActions = {
    updateCommentsForThread,
    changeThreadCommentOrder,
    addCommentToThread,
    toggleCommentLike
}

function updateCommentsForThread(threadId) {
    return (dispatch, getState) => {
        dispatch(request(threadId));

        const thread = findThreadInStoreState(threadId, getState());

        const client = new CommentsClient();
        client.comments(threadId, thread.commentOrder)
            .then(result => {
                const comments = result.comments.map(mapCommentModelToViewModel);
                dispatch(success(threadId, comments));
            });
    }

    function request(threadId) {
        return {type: threadsConstants.THREAD_COMMENTS_REQUESTED, threadId}
    }

    function success(threadId, comments) {
        return {
            type: threadsConstants.THREAD_COMMENTS_RECEIVED,
            threadId,
            comments
        }
    }
}

function changeThreadCommentOrder(threadId, order) {
    return dispatch => {
        dispatch(orderUpdate(threadId, order));
        dispatch(updateCommentsForThread(threadId));
    }

    function orderUpdate(threadId, order) {
        return {type: threadsConstants.THREAD_COMMENT_ORDER_CHANGED, threadId, order}
    }
}

function addCommentToThread(threadId, commentContent) {
    return dispatch => {
        dispatch(request(threadId));

        const client = new CommentsClient();
        client.create(threadId, commentContent)
            .then(result => {
                dispatch(success(threadId, commentContent));
                dispatch(updateCommentsForThread(threadId));
            });
    }

    function request(threadId) {
        return {type: threadsConstants.THREAD_COMMENT_ADD_REQUESTED, threadId}
    }

    function success(threadId, commentContent) {
        return {
            type: threadsConstants.THREAD_COMMENT_ADDED,
            threadId,
            commentContent
        }
    }
}

function toggleCommentLike(commentId) {
    return (dispatch, getState) => {
        const state = getState().commentsReducer;
        const comment = state.commentsById[commentId];

        if (comment.isChangingLikeStatus) return;

        dispatch(request(commentId));

        if (comment.likedByCurrentUser) {
            let client = new LikeCommentClient();
            client.unlike(commentId).then(
                () => {
                    dispatch(unlike(commentId));
                }
            );
        } else {
            let client = new LikeCommentClient();
            client.like(commentId).then(
                () => {
                    dispatch(like(commentId));
                }
            );
        }
    }

    function request(commentId) {
        return {type: threadsConstants.THREAD_COMMENT_LIKE_TOGGLE_REQUESTED, commentId}
    }

    function unlike(commentId) {
        return {type: commentsConstants.COMMENT_UNLIKED, commentId}
    }

    function like(commentId) {
        return {type: commentsConstants.COMMENT_LIKED, commentId}
    }
}