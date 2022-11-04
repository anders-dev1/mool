import {createReducer} from "@reduxjs/toolkit";
import {commentsConstants, threadsConstants} from "../actiontypes";

export const commentsReducer = createReducer(
    {
        commentsById:{},
        threadIdToCommentIds:{}
    },
    (builder) => {
        builder.addCase(threadsConstants.THREAD_COMMENTS_RECEIVED, (state, action) =>{
            action.comments.forEach(function(comment) {
               state.commentsById[comment.id] = comment;
            });

            state.threadIdToCommentIds[action.threadId] = action.comments.map(comment => comment.id);
        });
        builder.addCase(threadsConstants.THREAD_COMMENT_LIKE_TOGGLE_REQUESTED, (state, action) => {
            state.commentsById[action.commentId].isChangingLikeStatus = true;
        })
        builder.addCase(commentsConstants.COMMENT_LIKED, (state, action) => {
            const comment = state.commentsById[action.commentId];
            comment.isChangingLikeStatus = false;
            comment.likedByCurrentUser = true;
            comment.likes++;
        })
        builder.addCase(commentsConstants.COMMENT_UNLIKED, (state, action) => {
            const comment = state.commentsById[action.commentId];
            comment.isChangingLikeStatus = false;
            comment.likedByCurrentUser = false;
            comment.likes--;
        })
    }
)