import {CommentOrder} from "../api";

export function mapThreadModelToViewModel(thread) {
    return {
        id: thread.id,
        created: JSON.stringify(thread.created),
        author: thread.author,
        content: thread.content,
        likes: thread.likes,
        likedByCurrentUser: thread.likedByCurrentUser,
        numberOfComments: thread.comments,
        commentOrder: CommentOrder.OldestFirst,
        isChangingLikeStatus: false,
        showingCommentSection: false,
        fetchingComments: false,
        postingComment: false
    }
}

export function mapThreadsToViewModels(threads) {
    return threads.map((thread) => mapThreadModelToViewModel(thread));
}